#!/usr/bin/env python3
"""Generate Zone 1 building assets with Meshy Text to 3D.

This script is intentionally resumable: it records every paid Meshy task id in
art/_intermediate/zone1_meshy/meshy_zone1_state.json before polling/downloading.
"""

from __future__ import annotations

import argparse
import json
import os
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
from dataclasses import dataclass
from pathlib import Path
from typing import Any


API_ROOT = "https://api.meshy.ai/openapi"
TEXT_TO_3D_URL = f"{API_ROOT}/v2/text-to-3d"
BALANCE_URL = f"{API_ROOT}/v1/balance"
OUT_ROOT = Path("art/_intermediate/zone1_meshy")
STATE_PATH = OUT_ROOT / "meshy_zone1_state.json"

COMMON_STYLE = (
    "Low poly Meshy 6 model, HD-2D inspired flat shaded style, muted Anemora earth "
    "palette, clean topology, single base color texture, no PBR, no glow, no ornate "
    "fantasy, no futuristic parts, no religious symbols."
)


@dataclass(frozen=True)
class AssetSpec:
    asset_id: str
    group: str
    prompt: str
    texture_prompt: str
    final_id: str | None = None
    review_candidate: bool = False
    expected_credits: int = 30

    @property
    def out_dir(self) -> Path:
        return OUT_ROOT / self.group / self.asset_id


ASSETS: list[AssetSpec] = [
    AssetSpec(
        "House_Player_Candidate_01",
        "house_player",
        "Small simple wooden house, single story, 4 by 4 by 3 meters, sloped wooden roof, weathered muted brown plank walls, one dark wooden front door, no visible windows or boarded windows, simple stone foundation, gentle wear not ruined. " + COMMON_STYLE,
        "Weathered brown wooden planks, dark roof, simple stone foundation, quiet melancholic village palette, flat base color, no lighting baked in.",
        final_id="House_Player",
        review_candidate=True,
    ),
    AssetSpec(
        "House_Player_Candidate_02",
        "house_player",
        "Small lived-in wooden cottage, one story, about 4m square and 3m tall, simple steep plank roof, boarded tiny windows, single plain door, muted weathered browns, stone footing, gentle decay but inhabited. " + COMMON_STYLE,
        "Muted weathered wood, darker door, stone base, restrained decay, flat color texture, no baked lighting.",
        final_id="House_Player",
        review_candidate=True,
    ),
    AssetSpec(
        "House_Player_Candidate_03",
        "house_player",
        "Compact plain timber house, one story, 4x4x3 meters, slightly uneven sloped wooden roof, plank siding, one front door, windows absent or boarded, no decorations, used but not abandoned. " + COMMON_STYLE,
        "Dark brown and grey-brown wood, plain board texture, soft worn edges, flat base color, no glossy surface.",
        final_id="House_Player",
        review_candidate=True,
    ),
    AssetSpec(
        "House_Player_Candidate_04",
        "house_player",
        "Very simple low poly wooden home, single floor, roughly 4 by 4 by 3 meters, slanted plank roof, weathered wall boards, one darker door, boarded window hints, small stone foundation, no ornament. " + COMMON_STYLE,
        "Anemora muted browns, weathered planks, stone base, low saturation, flat texture, no baked shadows.",
        final_id="House_Player",
        review_candidate=True,
    ),
    AssetSpec(
        "Bed_Player",
        "house_player",
        "Simple single bed, 1.0 by 2.0 by 0.5 meters, dark brown wooden frame, plain beige linen blanket, one small pillow, slightly disheveled as if someone just got up, no decorations. " + COMMON_STYLE,
        "Dark wood frame, beige blanket, off-white pillow, slightly worn fabric, flat base color.",
    ),
    AssetSpec(
        "Bookshelf_Empty",
        "house_player",
        "Small plain wooden bookshelf, three shelves, dark walnut wood, about 1.0 by 1.5 by 0.3 meters, empty shelves, simple rectangular design, no carvings. " + COMMON_STYLE,
        "Dark walnut wood with subtle worn edges, empty shelves, flat base color.",
    ),
    AssetSpec(
        "Table_SmallChair_Wooden",
        "house_player",
        "Small wooden dining table for two with one simple wooden chair, square table top 0.8 by 0.8 meters, four legs, chair has straight back and no cushion, same dark wood tone. " + COMMON_STYLE,
        "Dark worn wood, simple plank surfaces, no cloth, no cushion, flat base color.",
    ),
    AssetSpec(
        "Door_House",
        "house_player",
        "Simple wooden plank door, vertical weathered dark brown boards, plain iron hinges and small iron handle, about 0.9 by 2.0 meters, standalone door prop. " + COMMON_STYLE,
        "Weathered dark wooden planks, dull iron hinge and handle, flat color, no shine.",
    ),
    AssetSpec(
        "Plaza_Monument_A_StoneBench",
        "plaza_center",
        "Weathered stone bench, single plain piece, about 2.0 by 0.5 by 0.5 meters, chipped edges, subtle dark green moss patches, quiet melancholic plaza prop. " + COMMON_STYLE,
        "Grey-brown stone, chipped edges, dark muted moss patches, flat base color.",
        review_candidate=True,
    ),
    AssetSpec(
        "Plaza_Fountain_Dry_Broken",
        "plaza_center",
        "Small abandoned stone fountain, circular basin about 2 meters diameter, no water, dry cracked stone bottom, central pillar broken at top, no ornate decoration, subtle moss. " + COMMON_STYLE,
        "Weathered grey-brown stone, dry cracked basin, muted moss, flat base color, no water.",
        review_candidate=True,
    ),
    AssetSpec(
        "Plaza_Monument_C_Pedestal",
        "plaza_center",
        "Indeterminate stone monument or pedestal, square base 1 by 1 meter, broken column on top about 1.5 meters tall with missing top, plain weathered stone, no inscriptions. " + COMMON_STYLE,
        "Plain weathered grey stone, chipped broken column, low saturation, flat base color.",
        review_candidate=True,
    ),
    AssetSpec(
        "StreetLamp",
        "plaza_center",
        "Simple wrought iron street lamp, total height 3 meters, single post with small lantern at top, lantern unlit, glass pane slightly cracked, dark iron color, reusable plaza prop. " + COMMON_STYLE,
        "Dark dull iron, slightly cracked dim glass, no emission, no shine, flat base color.",
    ),
    AssetSpec(
        "Tree_Decay",
        "plaza_center",
        "Small sparsely leafed tree, about 4 meters tall, slim trunk with a few twisted branches, very few faded yellow-brown leaves remaining, autumn decay look, reusable prop. " + COMMON_STYLE,
        "Brown-grey bark, sparse faded yellow-brown leaves, restrained decay, flat color.",
    ),
    AssetSpec(
        "Floor_Stone",
        "plaza_center",
        "Two by two meter stone floor tile patch, uneven low cobblestones, weathered grey-brown stones, mossy seams, tileable on all four sides, shallow ground mesh. " + COMMON_STYLE,
        "Grey-brown cobblestones, muted moss in seams, flat tileable base color.",
    ),
    AssetSpec(
        "Floor_Wood",
        "plaza_center",
        "Two by two meter wooden plank floor patch, dark parallel planks, slightly worn surface, tileable on all four sides, shallow ground mesh for house interior. " + COMMON_STYLE,
        "Dark worn wood planks, subtle grain, low saturation, flat tileable base color.",
    ),
    AssetSpec(
        "Library_Ruin",
        "library_ruin",
        "Small abandoned stone library, single story, about 6 by 6 by 5 meters, weathered light grey stone walls, wooden roof partly collapsed on one corner, closed wooden double doors, two boarded front windows, ivy on walls, no signage. " + COMMON_STYLE,
        "Light grey weathered stone, dark collapsed wood roof, boarded windows, muted ivy, flat base color, no baked lighting.",
    ),
    AssetSpec(
        "Bookshelf_Library_Past",
        "library_ruin",
        "Tall wooden library bookshelf, six shelves, dark wood, about 1.0 by 2.5 by 0.4 meters, filled with varied books in faded earth tones, plain design. " + COMMON_STYLE,
        "Dark wood shelf, books in ochre, dark green, dark red, beige, faded low saturation, flat color.",
    ),
    AssetSpec(
        "Book_Family_Past",
        "library_ruin",
        "Single old hardcover book, dark brown leather cover, faded gold-trim spine, about 0.2 by 0.3 by 0.05 meters, lying flat or standing on a shelf. " + COMMON_STYLE,
        "Dark brown leather, faded dull gold trim, worn pages, flat base color, no shine.",
    ),
]


class MeshyError(RuntimeError):
    pass


def load_state() -> dict[str, Any]:
    if not STATE_PATH.exists():
        return {
            "schema": 1,
            "source_spec": "docs/asset_prompts/zone1_buildings.md sections 2-4",
            "api_docs": "https://docs.meshy.ai/en/api/text-to-3d",
            "assets": {},
        }
    with STATE_PATH.open("r", encoding="utf-8") as f:
        return json.load(f)


def save_state(state: dict[str, Any]) -> None:
    OUT_ROOT.mkdir(parents=True, exist_ok=True)
    tmp = STATE_PATH.with_suffix(".json.tmp")
    with tmp.open("w", encoding="utf-8") as f:
        json.dump(state, f, ensure_ascii=False, indent=2)
        f.write("\n")
    tmp.replace(STATE_PATH)


def get_api_key() -> str:
    key = os.environ.get("MESHY_API_KEY", "").strip()
    if not key:
        raise MeshyError("MESHY_API_KEY is not set in this process.")
    return key


def http_json(method: str, url: str, key: str, payload: dict[str, Any] | None = None) -> Any:
    data = None
    headers = {"Authorization": f"Bearer {key}"}
    if payload is not None:
        data = json.dumps(payload).encode("utf-8")
        headers["Content-Type"] = "application/json"
    request = urllib.request.Request(url, data=data, headers=headers, method=method)
    try:
        with urllib.request.urlopen(request, timeout=60) as response:
            body = response.read().decode("utf-8")
            return json.loads(body) if body else {}
    except urllib.error.HTTPError as exc:
        body = exc.read().decode("utf-8", errors="replace")
        raise MeshyError(f"{method} {url} failed: HTTP {exc.code}: {body}") from exc
    except urllib.error.URLError as exc:
        raise MeshyError(f"{method} {url} failed: {exc}") from exc


def post_text_to_3d(key: str, payload: dict[str, Any]) -> str:
    data = http_json("POST", TEXT_TO_3D_URL, key, payload)
    task_id = data.get("result")
    if not isinstance(task_id, str) or not task_id:
        raise MeshyError(f"Unexpected create response: {data}")
    return task_id


def get_task(key: str, task_id: str) -> dict[str, Any]:
    data = http_json("GET", f"{TEXT_TO_3D_URL}/{urllib.parse.quote(task_id)}", key)
    if not isinstance(data, dict):
        raise MeshyError(f"Unexpected task response for {task_id}: {data}")
    return data


def wait_task(key: str, task_id: str, label: str, poll_seconds: int, timeout_minutes: int) -> dict[str, Any]:
    deadline = time.monotonic() + timeout_minutes * 60
    last_report: tuple[str, int | None] | None = None
    while True:
        task = get_task(key, task_id)
        status = str(task.get("status", "UNKNOWN"))
        progress_raw = task.get("progress")
        progress = progress_raw if isinstance(progress_raw, int) else None
        report = (status, progress)
        if report != last_report:
            if progress is None:
                print(f"{label}: {status}", flush=True)
            else:
                print(f"{label}: {status} {progress}%", flush=True)
            last_report = report

        if status == "SUCCEEDED":
            return task
        if status in {"FAILED", "EXPIRED", "CANCELED"}:
            raise MeshyError(f"{label} ended with {status}: {task.get('task_error')}")
        if time.monotonic() > deadline:
            raise MeshyError(f"{label} timed out after {timeout_minutes} minutes.")
        time.sleep(poll_seconds)


def write_json(path: Path, data: Any) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with path.open("w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
        f.write("\n")


def extension_from_url(url: str, fallback: str) -> str:
    parsed = urllib.parse.urlparse(url)
    suffix = Path(parsed.path).suffix.lower()
    if suffix and len(suffix) <= 8:
        return suffix
    return fallback


def download_url(url: str, path: Path) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    request = urllib.request.Request(url, headers={"User-Agent": "AnemoraAssetPipeline/1.0"})
    with urllib.request.urlopen(request, timeout=180) as response:
        path.write_bytes(response.read())


def download_outputs(spec: AssetSpec, stage: str, task: dict[str, Any]) -> dict[str, Any]:
    asset_dir = spec.out_dir
    asset_dir.mkdir(parents=True, exist_ok=True)
    downloaded: dict[str, Any] = {"models": {}, "textures": [], "thumbnail": None}

    model_urls = task.get("model_urls")
    if isinstance(model_urls, dict):
        for fmt, url in sorted(model_urls.items()):
            if fmt not in {"glb", "fbx", "obj", "mtl"}:
                continue
            if not isinstance(url, str) or not url:
                continue
            filename = f"{stage}_model.{fmt}"
            target = asset_dir / filename
            if not target.exists() or target.stat().st_size == 0:
                download_url(url, target)
            downloaded["models"][fmt] = str(target).replace("\\", "/")

    thumbnail_url = task.get("thumbnail_url")
    if isinstance(thumbnail_url, str) and thumbnail_url:
        target = asset_dir / f"{stage}_thumbnail{extension_from_url(thumbnail_url, '.png')}"
        if not target.exists() or target.stat().st_size == 0:
            download_url(thumbnail_url, target)
        downloaded["thumbnail"] = str(target).replace("\\", "/")

    texture_urls = task.get("texture_urls")
    if isinstance(texture_urls, list):
        for index, texture_set in enumerate(texture_urls):
            if not isinstance(texture_set, dict):
                continue
            saved_set: dict[str, str] = {}
            for kind, url in sorted(texture_set.items()):
                if not isinstance(url, str) or not url:
                    continue
                ext = extension_from_url(url, ".png")
                target = asset_dir / f"{stage}_texture_{index}_{kind}{ext}"
                if not target.exists() or target.stat().st_size == 0:
                    download_url(url, target)
                saved_set[kind] = str(target).replace("\\", "/")
            if saved_set:
                downloaded["textures"].append(saved_set)

    write_json(asset_dir / f"{stage}_task.json", task)
    write_json(asset_dir / f"{stage}_downloads.json", downloaded)
    return downloaded


def create_preview_payload(spec: AssetSpec) -> dict[str, Any]:
    return {
        "mode": "preview",
        "prompt": spec.prompt,
        "model_type": "lowpoly",
        "ai_model": "latest",
        "target_formats": ["glb", "fbx"],
        "auto_size": False,
        "moderation": False,
    }


def create_refine_payload(preview_task_id: str, spec: AssetSpec) -> dict[str, Any]:
    return {
        "mode": "refine",
        "preview_task_id": preview_task_id,
        "ai_model": "latest",
        "texture_prompt": spec.texture_prompt,
        "enable_pbr": False,
        "hd_texture": False,
        "remove_lighting": True,
        "target_formats": ["glb", "fbx"],
        "auto_size": False,
        "moderation": False,
    }


def ensure_entry(state: dict[str, Any], spec: AssetSpec) -> dict[str, Any]:
    assets = state.setdefault("assets", {})
    entry = assets.setdefault(spec.asset_id, {})
    entry.setdefault("asset_id", spec.asset_id)
    entry.setdefault("final_id", spec.final_id)
    entry.setdefault("group", spec.group)
    entry.setdefault("review_candidate", spec.review_candidate)
    entry.setdefault("expected_credits", spec.expected_credits)
    entry.setdefault("prompt", spec.prompt)
    entry.setdefault("texture_prompt", spec.texture_prompt)
    entry.setdefault("out_dir", str(spec.out_dir).replace("\\", "/"))
    return entry


def run_asset(key: str, state: dict[str, Any], spec: AssetSpec, args: argparse.Namespace) -> None:
    entry = ensure_entry(state, spec)
    spec.out_dir.mkdir(parents=True, exist_ok=True)

    preview = entry.setdefault("preview", {})
    if not preview.get("task_id"):
        payload = create_preview_payload(spec)
        write_json(spec.out_dir / "preview_payload.json", payload)
        print(f"CREATE preview {spec.asset_id}", flush=True)
        preview["task_id"] = post_text_to_3d(key, payload)
        preview["created_at"] = int(time.time())
        save_state(state)

    preview_task = wait_task(
        key,
        preview["task_id"],
        f"preview {spec.asset_id}",
        args.poll_seconds,
        args.timeout_minutes,
    )
    preview["status"] = preview_task.get("status")
    preview["consumed_credits"] = preview_task.get("consumed_credits")
    preview["downloads"] = download_outputs(spec, "preview", preview_task)
    save_state(state)

    refine = entry.setdefault("refine", {})
    if not refine.get("task_id"):
        payload = create_refine_payload(preview["task_id"], spec)
        write_json(spec.out_dir / "refine_payload.json", payload)
        print(f"CREATE refine {spec.asset_id}", flush=True)
        refine["task_id"] = post_text_to_3d(key, payload)
        refine["created_at"] = int(time.time())
        save_state(state)

    refine_task = wait_task(
        key,
        refine["task_id"],
        f"refine {spec.asset_id}",
        args.poll_seconds,
        args.timeout_minutes,
    )
    refine["status"] = refine_task.get("status")
    refine["consumed_credits"] = refine_task.get("consumed_credits")
    refine["downloads"] = download_outputs(spec, "refine", refine_task)
    save_state(state)


def create_preview_if_needed(key: str, state: dict[str, Any], spec: AssetSpec) -> None:
    entry = ensure_entry(state, spec)
    spec.out_dir.mkdir(parents=True, exist_ok=True)
    preview = entry.setdefault("preview", {})
    if preview.get("task_id"):
        return
    payload = create_preview_payload(spec)
    write_json(spec.out_dir / "preview_payload.json", payload)
    print(f"CREATE preview {spec.asset_id}", flush=True)
    preview["task_id"] = post_text_to_3d(key, payload)
    preview["created_at"] = int(time.time())
    save_state(state)


def wait_preview(key: str, state: dict[str, Any], spec: AssetSpec, args: argparse.Namespace) -> None:
    entry = ensure_entry(state, spec)
    preview = entry.setdefault("preview", {})
    task_id = preview.get("task_id")
    if not task_id:
        raise MeshyError(f"{spec.asset_id} preview has no task id.")
    if preview.get("status") == "SUCCEEDED" and preview.get("downloads"):
        return
    preview_task = wait_task(
        key,
        task_id,
        f"preview {spec.asset_id}",
        args.poll_seconds,
        args.timeout_minutes,
    )
    preview["status"] = preview_task.get("status")
    preview["consumed_credits"] = preview_task.get("consumed_credits")
    preview["downloads"] = download_outputs(spec, "preview", preview_task)
    save_state(state)


def create_refine_if_needed(key: str, state: dict[str, Any], spec: AssetSpec) -> None:
    entry = ensure_entry(state, spec)
    preview = entry.setdefault("preview", {})
    if preview.get("status") != "SUCCEEDED":
        raise MeshyError(f"{spec.asset_id} preview is not ready for refine.")
    refine = entry.setdefault("refine", {})
    if refine.get("task_id"):
        return
    payload = create_refine_payload(preview["task_id"], spec)
    write_json(spec.out_dir / "refine_payload.json", payload)
    print(f"CREATE refine {spec.asset_id}", flush=True)
    refine["task_id"] = post_text_to_3d(key, payload)
    refine["created_at"] = int(time.time())
    save_state(state)


def wait_refine(key: str, state: dict[str, Any], spec: AssetSpec, args: argparse.Namespace) -> None:
    entry = ensure_entry(state, spec)
    refine = entry.setdefault("refine", {})
    task_id = refine.get("task_id")
    if not task_id:
        raise MeshyError(f"{spec.asset_id} refine has no task id.")
    if refine.get("status") == "SUCCEEDED" and refine.get("downloads"):
        return
    refine_task = wait_task(
        key,
        task_id,
        f"refine {spec.asset_id}",
        args.poll_seconds,
        args.timeout_minutes,
    )
    refine["status"] = refine_task.get("status")
    refine["consumed_credits"] = refine_task.get("consumed_credits")
    refine["downloads"] = download_outputs(spec, "refine", refine_task)
    save_state(state)


def run_pipeline(key: str, state: dict[str, Any], selected: list[AssetSpec], args: argparse.Namespace) -> None:
    print("Creating preview tasks...", flush=True)
    for spec in selected:
        create_preview_if_needed(key, state, spec)
        time.sleep(1)

    print("Waiting for preview tasks...", flush=True)
    for spec in selected:
        wait_preview(key, state, spec, args)

    print("Creating refine tasks...", flush=True)
    for spec in selected:
        create_refine_if_needed(key, state, spec)
        time.sleep(1)

    print("Waiting for refine tasks...", flush=True)
    for spec in selected:
        wait_refine(key, state, spec, args)


def parse_args(argv: list[str]) -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--only", nargs="*", help="Asset ids to generate. Defaults to all.")
    parser.add_argument("--poll-seconds", type=int, default=15)
    parser.add_argument("--timeout-minutes", type=int, default=25)
    parser.add_argument("--balance-only", action="store_true")
    parser.add_argument("--sequential", action="store_true", help="Run preview/refine asset by asset.")
    return parser.parse_args(argv)


def main(argv: list[str]) -> int:
    args = parse_args(argv)
    key = get_api_key()
    OUT_ROOT.mkdir(parents=True, exist_ok=True)
    state = load_state()
    balance = http_json("GET", BALANCE_URL, key)
    state["balance_before_or_last"] = balance
    save_state(state)
    print(f"Meshy balance: {balance}", flush=True)
    if args.balance_only:
        return 0

    selected = ASSETS
    if args.only:
        wanted = set(args.only)
        selected = [spec for spec in ASSETS if spec.asset_id in wanted]
        missing = sorted(wanted - {spec.asset_id for spec in selected})
        if missing:
            raise MeshyError(f"Unknown asset ids: {', '.join(missing)}")

    expected = sum(spec.expected_credits for spec in selected)
    print(f"Generating {len(selected)} assets; expected maximum {expected} credits.", flush=True)
    for spec in selected:
        if len(spec.prompt) > 600:
            raise MeshyError(f"{spec.asset_id} prompt is {len(spec.prompt)} chars; Meshy limit is 600.")
        if len(spec.texture_prompt) > 600:
            raise MeshyError(f"{spec.asset_id} texture prompt is {len(spec.texture_prompt)} chars; Meshy limit is 600.")

    if args.sequential:
        for spec in selected:
            run_asset(key, state, spec, args)
    else:
        run_pipeline(key, state, selected, args)

    state["completed_at"] = int(time.time())
    state["balance_after_or_last"] = http_json("GET", BALANCE_URL, key)
    save_state(state)
    print(f"Meshy balance after: {state['balance_after_or_last']}", flush=True)
    return 0


if __name__ == "__main__":
    try:
        raise SystemExit(main(sys.argv[1:]))
    except MeshyError as exc:
        print(f"ERROR: {exc}", file=sys.stderr)
        raise SystemExit(1)
