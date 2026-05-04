#!/usr/bin/env python3
"""Blender post-process for Meshy Zone 1 building outputs.

Run with Blender:
  blender --background --python tools/postprocess_meshy_zone1_buildings.py
"""

from __future__ import annotations

import json
import math
import re
import sys
from dataclasses import dataclass
from datetime import date
from pathlib import Path
from typing import Any

import bpy
from mathutils import Vector


REPO_ROOT = Path(__file__).resolve().parents[1]
MESHY_ROOT = REPO_ROOT / "art/_intermediate/zone1_meshy"
MODEL_ROOT = REPO_ROOT / "Assets/Art/Models/Zone1"
TEXTURE_ROOT = MODEL_ROOT / "Textures"
MANIFEST_PATH = MODEL_ROOT / "zone1_buildings_manifest.json"
STATE_PATH = MESHY_ROOT / "meshy_zone1_state.json"


@dataclass(frozen=True)
class FinalAsset:
    asset_id: str
    category: str
    source_asset_id: str | None
    source_group: str | None
    output_subdir: str
    target_size: tuple[float, float, float]
    max_triangles: int
    notes: str
    procedural_rebuild: str | None = None
    add_family_books: bool = False

    @property
    def output_path(self) -> Path:
        return MODEL_ROOT / self.output_subdir / f"{self.asset_id}.fbx"

    @property
    def source_path(self) -> Path | None:
        if not self.source_asset_id or not self.source_group:
            return None
        return MESHY_ROOT / self.source_group / self.source_asset_id / "refine_model.glb"


FINAL_ASSETS: list[FinalAsset] = [
    FinalAsset(
        "House_Player",
        "HousePlayer",
        "House_Player_Candidate_03",
        "house_player",
        "HousePlayer",
        (4.25, 4.0, 3.0),
        1200,
        "Meshy candidate 03 selected for draft; candidates 01/02 visibly broken, candidate 04 retained for review.",
    ),
    FinalAsset(
        "Bed_Player",
        "HousePlayer",
        "Bed_Player",
        "house_player",
        "HousePlayer",
        (1.05, 2.05, 0.65),
        400,
        "Simple disheveled single bed from section 2.3.1.",
    ),
    FinalAsset(
        "Bookshelf_Empty",
        "HousePlayer",
        "Bookshelf_Empty",
        "house_player",
        "HousePlayer",
        (1.0, 0.3, 1.5),
        500,
        "Meshy output had a diagonal board artifact; shelf was rebuilt in Blender to preserve the prompt intent.",
        procedural_rebuild="bookshelf_empty",
    ),
    FinalAsset(
        "Bookshelf_FamilyBooks",
        "HousePlayer",
        "Bookshelf_Empty",
        "house_player",
        "HousePlayer",
        (1.0, 0.3, 1.5),
        650,
        "Family-books variant built from the corrected empty shelf with 8 muted book blocks.",
        procedural_rebuild="bookshelf_empty",
        add_family_books=True,
    ),
    FinalAsset(
        "Table_SmallChair_Wooden",
        "HousePlayer",
        "Table_SmallChair_Wooden",
        "house_player",
        "HousePlayer",
        (1.2, 1.55, 1.15),
        700,
        "Small table and wooden chair from section 2.3.3.",
    ),
    FinalAsset(
        "Door_House",
        "HousePlayer",
        "Door_House",
        "house_player",
        "HousePlayer",
        (0.9, 0.08, 2.0),
        300,
        "Standalone plank door from section 2.3.4.",
    ),
    FinalAsset(
        "Plaza_Fountain_Dry_Broken",
        "Plaza",
        "Plaza_Fountain_Dry_Broken",
        "plaza_center",
        "Plaza",
        (2.0, 2.0, 1.2),
        900,
        "Plaza monument option B selected as draft; A/C kept in Meshy intermediate for review.",
    ),
    FinalAsset(
        "StreetLamp",
        "Plaza",
        "StreetLamp",
        "plaza_center",
        "Plaza",
        (0.45, 0.45, 3.0),
        400,
        "Reusable unlit cracked street lamp from section 3.3.1.",
    ),
    FinalAsset(
        "Tree_Decay",
        "Plaza",
        "Tree_Decay",
        "plaza_center",
        "Plaza",
        (1.45, 1.45, 4.0),
        1000,
        "Sparse-leaf decay tree from section 3.3.2; leaf amount awaits user review.",
    ),
    FinalAsset(
        "Floor_Stone",
        "Plaza",
        "Floor_Stone",
        "plaza_center",
        "Plaza",
        (2.0, 2.0, 0.08),
        400,
        "Meshy stone tile was too dense for the target; rebuilt in Blender as low-poly uneven cobblestones.",
        procedural_rebuild="floor_stone",
    ),
    FinalAsset(
        "Floor_Wood",
        "Plaza",
        "Floor_Wood",
        "plaza_center",
        "Plaza",
        (2.0, 2.0, 0.05),
        200,
        "2m tileable wooden floor patch from section 3.4.2.",
    ),
    FinalAsset(
        "Library_Ruin",
        "LibraryRuin",
        "Library_Ruin",
        "library_ruin",
        "LibraryRuin",
        (6.0, 6.0, 5.0),
        1500,
        "Library ruin exterior from section 4.2.",
    ),
    FinalAsset(
        "Bookshelf_Library_Past",
        "LibraryRuin",
        "Bookshelf_Library_Past",
        "library_ruin",
        "LibraryRuin",
        (1.0, 0.4, 2.5),
        1100,
        "Filled library bookshelf from section 4.3.1.",
    ),
    FinalAsset(
        "Book_Family_Past",
        "LibraryRuin",
        "Book_Family_Past",
        "library_ruin",
        "LibraryRuin",
        (0.2, 0.3, 0.05),
        200,
        "Interactable old family book from section 4.3.2.",
    ),
]


PALETTE = {
    "wood_dark": (0.22, 0.13, 0.09, 1.0),
    "wood_mid": (0.34, 0.22, 0.15, 1.0),
    "book_red": (0.35, 0.12, 0.10, 1.0),
    "book_green": (0.13, 0.24, 0.17, 1.0),
    "book_ochre": (0.45, 0.32, 0.15, 1.0),
    "book_beige": (0.56, 0.48, 0.35, 1.0),
}


def clean_scene() -> None:
    bpy.ops.object.mode_set(mode="OBJECT") if bpy.ops.object.mode_set.poll() else None
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()
    for block in (bpy.data.meshes, bpy.data.materials, bpy.data.images, bpy.data.textures):
        for item in list(block):
            if item.users == 0:
                block.remove(item)


def make_mat(name: str, color: tuple[float, float, float, float]) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.diffuse_color = color
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        if "Base Color" in bsdf.inputs:
            bsdf.inputs["Base Color"].default_value = color
        if "Metallic" in bsdf.inputs:
            bsdf.inputs["Metallic"].default_value = 0.0
        if "Roughness" in bsdf.inputs:
            bsdf.inputs["Roughness"].default_value = 0.95
        if "Alpha" in bsdf.inputs:
            bsdf.inputs["Alpha"].default_value = color[3]
    return mat


def cube(name: str, loc: tuple[float, float, float], scale: tuple[float, float, float], mat: bpy.types.Material) -> None:
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=loc)
    obj = bpy.context.object
    obj.name = name
    obj.dimensions = scale
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    obj.data.materials.append(mat)


def build_bookshelf(add_books: bool) -> None:
    wood = make_mat("Anemora_Walnut_Wood", PALETTE["wood_dark"])
    side_w = 0.06
    shelf_h = 0.05
    width = 1.0
    depth = 0.3
    height = 1.5
    cube("Shelf_Back", (0, 0.145, height / 2), (width, 0.03, height), wood)
    cube("Shelf_Left", (-width / 2 + side_w / 2, 0, height / 2), (side_w, depth, height), wood)
    cube("Shelf_Right", (width / 2 - side_w / 2, 0, height / 2), (side_w, depth, height), wood)
    for z in (0.05, 0.52, 0.98, 1.45):
        cube(f"Shelf_Board_{z:.2f}", (0, 0, z), (width, depth, shelf_h), wood)

    if add_books:
        mats = [
            make_mat("Book_Muted_Red", PALETTE["book_red"]),
            make_mat("Book_Muted_Green", PALETTE["book_green"]),
            make_mat("Book_Ochre", PALETTE["book_ochre"]),
            make_mat("Book_Beige", PALETTE["book_beige"]),
        ]
        specs = [
            (-0.36, -0.08, 0.27, 0.08, 0.14, 0.35),
            (-0.25, -0.08, 0.25, 0.07, 0.14, 0.30),
            (-0.08, -0.08, 0.29, 0.10, 0.14, 0.38),
            (0.08, -0.08, 0.73, 0.07, 0.14, 0.34),
            (0.19, -0.08, 0.71, 0.08, 0.14, 0.30),
            (0.34, -0.08, 0.75, 0.09, 0.14, 0.39),
            (-0.31, -0.08, 1.17, 0.09, 0.14, 0.33),
            (-0.17, -0.08, 1.20, 0.08, 0.14, 0.39),
        ]
        for index, (x, y, z, sx, sy, sz) in enumerate(specs):
            cube(f"FamilyBook_{index + 1:02d}", (x, y, z), (sx, sy, sz), mats[index % len(mats)])


def build_floor_stone() -> None:
    stone_a = make_mat("Stone_Grey_Brown_A", (0.38, 0.36, 0.32, 1.0))
    stone_b = make_mat("Stone_Grey_Brown_B", (0.30, 0.29, 0.26, 1.0))
    stone_c = make_mat("Stone_Grey_Moss", (0.24, 0.30, 0.22, 1.0))
    mats = [stone_a, stone_b, stone_a, stone_c]
    cols = 5
    rows = 4
    for y in range(rows):
        for x in range(cols):
            width = 0.36 + (0.03 if (x + y) % 2 == 0 else -0.02)
            depth = 0.42 + (0.02 if x % 3 == 0 else -0.015)
            height = 0.045 + 0.01 * ((x * 2 + y) % 3)
            loc_x = -0.8 + x * 0.4 + (0.02 if y % 2 else -0.01)
            loc_y = -0.75 + y * 0.5 + (0.015 if x % 2 else -0.015)
            cube(f"Stone_{x}_{y}", (loc_x, loc_y, height / 2.0), (width, depth, height), mats[(x + y) % len(mats)])


def import_source(asset: FinalAsset) -> None:
    source_path = asset.source_path
    if not source_path or not source_path.exists():
        raise FileNotFoundError(f"Missing source GLB for {asset.asset_id}: {source_path}")
    bpy.ops.import_scene.gltf(filepath=str(source_path))
    for obj in list(bpy.context.scene.objects):
        if obj.type not in {"MESH"}:
            bpy.data.objects.remove(obj, do_unlink=True)


def mesh_objects() -> list[bpy.types.Object]:
    return [obj for obj in bpy.context.scene.objects if obj.type == "MESH"]


def bounds() -> tuple[Vector, Vector]:
    objs = mesh_objects()
    if not objs:
        return Vector((0, 0, 0)), Vector((0, 0, 0))
    mins = Vector((math.inf, math.inf, math.inf))
    maxs = Vector((-math.inf, -math.inf, -math.inf))
    for obj in objs:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            mins.x = min(mins.x, point.x)
            mins.y = min(mins.y, point.y)
            mins.z = min(mins.z, point.z)
            maxs.x = max(maxs.x, point.x)
            maxs.y = max(maxs.y, point.y)
            maxs.z = max(maxs.z, point.z)
    return mins, maxs


def center_and_scale(target_size: tuple[float, float, float]) -> None:
    mins, maxs = bounds()
    size = maxs - mins
    scale = Vector((
        target_size[0] / size.x if size.x else 1.0,
        target_size[1] / size.y if size.y else 1.0,
        target_size[2] / size.z if size.z else 1.0,
    ))
    for obj in mesh_objects():
        obj.scale.x *= scale.x
        obj.scale.y *= scale.y
        obj.scale.z *= scale.z
    bpy.ops.object.select_all(action="DESELECT")
    for obj in mesh_objects():
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
    if mesh_objects():
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)

    mins, maxs = bounds()
    offset = Vector((-(mins.x + maxs.x) / 2.0, -(mins.y + maxs.y) / 2.0, -mins.z))
    for obj in mesh_objects():
        obj.location += offset
    bpy.context.view_layer.update()


def triangle_count() -> int:
    count = 0
    depsgraph = bpy.context.evaluated_depsgraph_get()
    for obj in mesh_objects():
        evaluated = obj.evaluated_get(depsgraph)
        mesh = evaluated.to_mesh()
        mesh.calc_loop_triangles()
        count += len(mesh.loop_triangles)
        evaluated.to_mesh_clear()
    return count


def apply_mesh_cleanup() -> None:
    for obj in mesh_objects():
        bpy.ops.object.mode_set(mode="OBJECT") if bpy.ops.object.mode_set.poll() else None
        bpy.ops.object.select_all(action="DESELECT")
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        bpy.ops.object.mode_set(mode="EDIT")
        bpy.ops.mesh.select_all(action="SELECT")
        bpy.ops.mesh.remove_doubles(threshold=0.001)
        bpy.ops.mesh.normals_make_consistent(inside=False)
        bpy.ops.object.mode_set(mode="OBJECT")
        for poly in obj.data.polygons:
            poly.use_smooth = False
        obj.data.update()


def decimate_to(max_triangles: int) -> None:
    for pass_index in range(4):
        current = triangle_count()
        if current <= max_triangles or current == 0:
            return
        ratio = min(max((max_triangles / current) * 0.85, 0.005), 1.0)
        for obj in mesh_objects():
            if not obj.data.polygons:
                continue
            bpy.ops.object.select_all(action="DESELECT")
            obj.select_set(True)
            bpy.context.view_layer.objects.active = obj
            mod = obj.modifiers.new(f"Zone1_Decimate_{pass_index + 1}", "DECIMATE")
            mod.ratio = ratio
            mod.use_collapse_triangulate = True
            bpy.ops.object.modifier_apply(modifier=mod.name)


def triangulate_all() -> None:
    for obj in mesh_objects():
        bpy.ops.object.select_all(action="DESELECT")
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        mod = obj.modifiers.new("Zone1_Triangulate", "TRIANGULATE")
        mod.quad_method = "BEAUTY"
        mod.ngon_method = "BEAUTY"
        bpy.ops.object.modifier_apply(modifier=mod.name)


def sanitize(value: str) -> str:
    return re.sub(r"[^A-Za-z0-9_.-]+", "_", value).strip("_") or "Texture"


def tune_materials_and_textures(asset_id: str) -> list[dict[str, Any]]:
    TEXTURE_ROOT.mkdir(parents=True, exist_ok=True)
    shader_notes: list[dict[str, Any]] = []
    for mat in bpy.data.materials:
        mat.use_nodes = True
        bsdf = mat.node_tree.nodes.get("Principled BSDF")
        if bsdf:
            if "Metallic" in bsdf.inputs:
                bsdf.inputs["Metallic"].default_value = 0.0
            if "Roughness" in bsdf.inputs:
                bsdf.inputs["Roughness"].default_value = 0.95

    for index, image in enumerate(bpy.data.images):
        if image.type != "IMAGE" or image.size[0] == 0 or image.size[1] == 0:
            continue
        width, height = int(image.size[0]), int(image.size[1])
        if max(width, height) > 1024:
            if width >= height:
                new_w = 1024
                new_h = max(1, int(height * 1024 / width))
            else:
                new_h = 1024
                new_w = max(1, int(width * 1024 / height))
            image.scale(new_w, new_h)
            width, height = new_w, new_h

        # Slightly desaturate/soften Meshy textures for the Anemora muted palette.
        pixels = list(image.pixels)
        for i in range(0, len(pixels), 4):
            r, g, b = pixels[i], pixels[i + 1], pixels[i + 2]
            grey = (r + g + b) / 3.0
            pixels[i] = min(max((grey + (r - grey) * 0.78) * 0.92, 0.0), 1.0)
            pixels[i + 1] = min(max((grey + (g - grey) * 0.78) * 0.92, 0.0), 1.0)
            pixels[i + 2] = min(max((grey + (b - grey) * 0.78) * 0.92, 0.0), 1.0)
        image.pixels[:] = pixels

        filename = f"{asset_id}_{index}_{sanitize(image.name)}.png"
        path = TEXTURE_ROOT / filename
        image.filepath_raw = str(path)
        image.file_format = "PNG"
        try:
            image.save()
            shader_notes.append({"path": str(path.relative_to(REPO_ROOT)).replace("\\", "/"), "size": f"{width}x{height}"})
        except RuntimeError:
            shader_notes.append({"path": f"<embedded:{image.name}>", "size": f"{width}x{height}"})
    return shader_notes


def export_fbx(path: Path) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in mesh_objects():
        obj.select_set(True)
    if mesh_objects():
        bpy.context.view_layer.objects.active = mesh_objects()[0]
        bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    bpy.ops.export_scene.fbx(
        filepath=str(path),
        use_selection=True,
        apply_unit_scale=True,
        apply_scale_options="FBX_SCALE_NONE",
        object_types={"MESH"},
        mesh_smooth_type="EDGE",
        path_mode="COPY",
        embed_textures=True,
        add_leaf_bones=False,
        bake_anim=False,
        use_mesh_modifiers=True,
    )


def process_asset(asset: FinalAsset, state: dict[str, Any]) -> dict[str, Any]:
    clean_scene()
    if asset.procedural_rebuild == "bookshelf_empty":
        build_bookshelf(asset.add_family_books)
    elif asset.procedural_rebuild == "floor_stone":
        build_floor_stone()
    else:
        import_source(asset)

    center_and_scale(asset.target_size)
    apply_mesh_cleanup()
    decimate_to(asset.max_triangles)
    triangulate_all()
    decimate_to(asset.max_triangles)
    triangulate_all()
    apply_mesh_cleanup()
    center_and_scale(asset.target_size)
    textures = tune_materials_and_textures(asset.asset_id)
    export_fbx(asset.output_path)

    mins, maxs = bounds()
    size = maxs - mins
    source_entry = None
    if asset.source_asset_id:
        source_entry = state.get("assets", {}).get(asset.source_asset_id, {})
    return {
        "id": asset.asset_id,
        "category": asset.category,
        "path": str(asset.output_path.relative_to(REPO_ROOT)).replace("\\", "/"),
        "source_meshy_asset_id": asset.source_asset_id,
        "source_preview_task_id": (source_entry.get("preview") or {}).get("task_id") if source_entry else None,
        "source_refine_task_id": (source_entry.get("refine") or {}).get("task_id") if source_entry else None,
        "triangles": triangle_count(),
        "objects": len(mesh_objects()),
        "bounds": {
            "min": [round(mins.x, 3), round(mins.y, 3), round(mins.z, 3)],
            "max": [round(maxs.x, 3), round(maxs.y, 3), round(maxs.z, 3)],
            "size_m": [round(size.x, 3), round(size.y, 3), round(size.z, 3)],
        },
        "texture": textures[0]["path"] if textures else "flat Blender materials",
        "texture_size": textures[0]["size"] if textures else "N/A",
        "textures": textures,
        "max_triangles_target": asset.max_triangles,
        "notes": asset.notes,
    }


def write_manifest(assets: list[dict[str, Any]], state: dict[str, Any]) -> None:
    total_preview = 0
    total_refine = 0
    for entry in state.get("assets", {}).values():
        total_preview += int((entry.get("preview") or {}).get("consumed_credits") or 0)
        total_refine += int((entry.get("refine") or {}).get("consumed_credits") or 0)
    manifest = {
        "generated_date": str(date.today()),
        "source_spec": "docs/asset_prompts/zone1_buildings.md sections 2-4 and 8",
        "tool": "Meshy Text to 3D API (Meshy 6 latest, lowpoly) + Blender 4.5.5 LTS",
        "meshy_status": "18 preview/refine tasks succeeded",
        "meshy_credits": {
            "preview": total_preview,
            "refine": total_refine,
            "total": total_preview + total_refine,
            "balance_before": state.get("balance_before_or_last"),
            "balance_after": state.get("balance_after_or_last"),
        },
        "intermediate_root": "art/_intermediate/zone1_meshy",
        "texture_policy": {
            "pbr": False,
            "max_base_color_size": "1024px longest side",
            "palette": "Meshy base color textures desaturated/softened toward Anemora muted earth palette; Blender repair assets use flat palette materials.",
        },
        "blender_checklist": {
            "scale_normalized_meters": True,
            "merge_by_distance_m": 0.001,
            "normals_recalculated_outside": True,
            "uv_texture_preserved_or_blender_rebuilt": True,
            "palette_integrated": "Muted Anemora earth palette pass; metallic=0, roughness=0.95.",
            "polygon_counts_recorded": True,
            "fbx_export": "Apply unit scale, smoothing edge, path mode COPY, embedded textures.",
            "unity_import": "Unity batch setup imports model materials, sets URP/Lit defaults, and creates prefabs under Assets/Prefabs/Zone1.",
        },
        "selected_candidates": {
            "House_Player": "House_Player_Candidate_03",
            "Plaza_Center_Monument": "Plaza_Fountain_Dry_Broken (option B draft)",
        },
        "review_candidates": {
            "House_Player": [
                "House_Player_Candidate_01",
                "House_Player_Candidate_02",
                "House_Player_Candidate_03",
                "House_Player_Candidate_04",
            ],
            "Plaza_Center_Monument": [
                "Plaza_Monument_A_StoneBench",
                "Plaza_Fountain_Dry_Broken",
                "Plaza_Monument_C_Pedestal",
            ],
        },
        "assets": assets,
        "review_pending": [
            "Plaza_Center monument final choice: B dry fountain is exported as draft; A/C remain in Meshy intermediate for user review.",
            "Tree_Decay leaf fall amount: sparse/near-leafless draft awaits user review.",
            "House_Player interior scope: Bed, Bookshelf variants, Table+Chair, and Door are included as draft scope.",
        ],
    }
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8") as f:
        json.dump(manifest, f, ensure_ascii=False, indent=2)
        f.write("\n")


def main() -> int:
    state = json.loads(STATE_PATH.read_text(encoding="utf-8"))
    results: list[dict[str, Any]] = []
    for asset in FINAL_ASSETS:
        print(f"Postprocess {asset.asset_id}", flush=True)
        results.append(process_asset(asset, state))
    write_manifest(results, state)
    print(f"Wrote {MANIFEST_PATH}", flush=True)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
