# Fast VS Story Canon v4 Source Check

Date: 2026-05-18

Project:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`

## Trigger

ユーザーから、現在の Fast VS が古いストーリー原本を参照している可能性があり、「親子」要素もあったはずだという指摘があった。

## Checked Sources

- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_scene1_v3_final.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_layer1_revision_and_scene3_design.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\draft\chapter1_s1_s2_handover_2026-05-08.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-12_chapter1_vs_story_canon_inventory.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-runtime\docs\draft\chapter1_graphic_session_handover_2026-05-09.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-runtime\docs\draft\chapter1_map_handover_2026-05-08.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-runtime\docs\DESIGN_RATIONALE.md`

## Finding

The current implementation had drifted across incompatible source versions.

`C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-12_chapter1_vs_story_canon_inventory.md` is not the latest Scene 1 source. It explicitly treats `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_scene1_v3_final.md` as a provisional VS reference until a later revision is selected.

The later revision is Scene 1 v4, recorded in:

- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_layer1_revision_and_scene3_design.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\draft\chapter1_s1_s2_handover_2026-05-08.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-runtime\docs\draft\chapter1_graphic_session_handover_2026-05-09.md`

## Adopted Canon For Fast VS

Fast VS should adopt Scene 1 v4:

- Niro enters the past library through the time window.
- Layer 1 permits walking, touching, and local trace changes.
- Niro takes the past book, `家族の記録` / B-γ.
- The book appears on the current desk.
- Reto reaction is restored: `...本物だ`, `...そうですか`, `...あなたのような方が、来てくれるとは`.
- Mia hint final line should be `あなたなら、力になれるかもしれません`.
- Scene 1 Aria is a distant past figure in the library and does not notice Niro.

## Parent / Child Clarification

The remembered parent/child material is real, but it belongs to Scene 3, not Scene 1.

- Scene 3 [3.D] has Aria and Karla as mother/daughter in the past Aria house.
- The Scene 3 material includes Karla teaching Aria trade/merchant work.
- Scene 1 should not include Aria/Karla mother-daughter dialogue.
- Scene 1 should keep Aria as distant library presence only.

Related hidden foreshadowing:

- Niro house past has signs of another family.
- That is a separate hidden Chapter 1 service element and should not be folded into the library Reto event.

## Implementation Impact

The following current Fast VS story elements are incorrect or provisional under the v4 source decision:

- Aria saying `レトさん` is invalid.
- A direct Aria/Niro conversation is invalid because past NPCs do not notice Niro.
- Any Scene 1 mother/daughter dialogue is invalid.
- `もし手があるなら、少し、助けてやってください` is the older v3 Mia hint line and should be replaced by the v4 line.
- The v3 no-book-return logic is no longer the target for Fast VS.

## Worker Instruction

The gpt-5.4-mini story/UI worker was redirected to this v4 canon basis and told to report any remaining provisional Aria wording.

