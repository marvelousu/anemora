# G3 Final Dialogue Self-Audit

Target: `da6040f Add G3 final dialogue draft for vertical slice`

Audit date: 2026-05-06

Scope: final G3 dialogue strings in `Assets/Localization/StringTables/`, `Assets/ScriptableObjects/Dialogues/`, and the `NiroMonologueController` display hook. No dialogue text was changed in this audit.

## Summary

| Viewpoint | Verdict | Notes |
|---|---|---|
| CONCEPT alignment | ⚠️ | Character and NPC role fit is strong; the true-cause / deeper-rule nudge is intentionally subtle but currently weak. |
| Forbidden term leakage | ✅ | No player-facing `層`, `ベール剥離`, `メタ④`, `観測者`, `第 N 層`, `dialogue.placeholder.*`, or `[TBD: Resident...]` found in scoped runtime dialogue assets. |
| Vocabulary / tone | ✅ | Neutral Niro wording, restrained mood, no emoji, no industry slang, no family / acquaintance claim. |
| ja-JP / en alignment | ⚠️ | Mostly natural; two minor nuance drifts are worth a Stage 4 copy pass. |
| Key naming | ⚠️ | Runtime keys are consistent and migrated; contributor API doc still has stale placeholder examples outside this task's edit scope. |

## 1. CONCEPT Alignment

**Verdict: ⚠️**

Evidence:

- Niro spec fit: `SPEC.md` defines Niro as neutral, 15-19, quiet, silent protagonist, Antela resident, with no family / acquaintances (`SPEC.md` §4.1). The current lines avoid gendered pronouns and family / acquaintance anchors:
  - `dialogue.niro.intro.line_2`: 「誰かが残したはずの道なのに、歩くたび、少しずつ遠くなる。」
  - `dialogue.niro.intro.line_3`: 「返せるものがあるなら、返したい。」
  - `dialogue.niro.intro.line_4`: 「防げる終わりがあるなら、まだ間に合うと思いたい。」
- Niro past portal fit: the moment is internal and sensory, matching the "past portal glimpse" scope without using design terms:
  - `dialogue.niro.past_portal.line_1`: 「街が、息をしている。」
  - `dialogue.niro.past_portal.line_2`: 「知らないはずの温度が、手のひらに残る。」
- Resident_A role fit: `STAGE3_TBD_RESOLUTION.md` fixes Resident_A as a past-side resident, no prior relationship, witness / hook pointing toward the ruins / library. Current text supports this:
  - `dialogue.encounter.past_resident_a.line_1`: 「あれ、見えてる？　向こうの大きな建物。」
  - `dialogue.encounter.past_resident_a.line_2`: 「昨日まで灯りがついてたのに、今日は窓が黒いの。」
  - `dialogue.encounter.past_resident_a.line_3`: 「近づくなら、足音を小さくして。あそこ、まだ聞いてる気がする。」
- Resident_B role fit: `STAGE3_TBD_RESOLUTION.md` fixes Resident_B as the current-side ruins / library observer-recorder, no prior relationship. The localized speaker label avoids the forbidden player-facing term and uses `記録者` / `Recorder`:
  - `dialogue.encounter.present_resident_b.line_1`: 「ここでは、崩れた順番だけがまだ残っている。」
  - `dialogue.encounter.present_resident_b.line_3`: 「誰が来たかは記録しない。何が減ったかだけを書いている。」

Assessment:

- Niro's text aligns with neutral protagonist constraints. It uses `I` in English, but not gendered pronouns, and the Japanese first-person issue is avoided entirely.
- Resident_A and Resident_B match their VS roles and do not imply prior familiarity with Niro.
- The surface decay layer is clear through dust, black windows, collapse order, and records of things decreasing.
- The deeper true-cause structure is only hinted by "まだ聞いてる", "知らないはずの温度", and "何が減ったか". This is probably acceptable for VS minimum copy, but weak if the intended player takeaway is "there is a second rule beneath ordinary decay."

Improvement proposal:

- In the Stage 4 v1 copy pass, add or replace one line with a non-terminological "looking / remaining / decreasing" hint, for example a Resident_B-style line such as: 「目を向けたあとに、減っているものがある。」 / "After we look, something is always less." This nudges the true-cause direction without using `観測者`, `層`, or `ベール剥離`.

## 2. Forbidden Term Leakage

**Verdict: ✅**

Checks performed:

```powershell
rg -n "層|ベール剥離|メタ④|観測者|第 [0-9N]|第[0-9N]|dialogue\.placeholder" Assets\Localization Assets\ScriptableObjects\Dialogues Assets\Scripts\TimeManagement\NiroMonologueController.cs
rg -n "dialogue\.placeholder|\[TBD: Resident" Assets
```

Result: no matches in the scoped runtime dialogue assets.

Concrete scoped text examples:

- `dialogue.niro.past_portal.line_2`: 「知らないはずの温度が、手のひらに残る。」
- `dialogue.encounter.present_resident_b.line_3`: 「誰が来たかは記録しない。何が減ったかだけを書いている。」

Assessment:

- `NiroMonologueController.cs` contains the runtime display hook and reflection-based `DialogueDisplay.Show` call; it does not contain player-facing dialogue text.
- `dialogue.speaker.resident_b` resolves to `記録者` / `Recorder`, not `観測者` / `Observer`.
- `dialogue.placeholder.*` and `[TBD: Resident...]` are absent from `Assets`.

Improvement proposal:

- No runtime dialogue change required for this point. Keep the same `rg` check as a pre-commit guard whenever player-facing dialogue changes.

## 3. Vocabulary / Tone

**Verdict: ✅**

Evidence:

- Niro stays quiet, sensory, and internally motivated:
  - `dialogue.niro.intro.line_1`: 「アンテラの朝は、音より先に埃が動く。」
  - `dialogue.niro.intro.line_4`: 「防げる終わりがあるなら、まだ間に合うと思いたい。」
- Resident_A has an anxious but grounded child / young-resident tone:
  - `dialogue.encounter.past_resident_a.line_2`: 「昨日まで灯りがついてたのに、今日は窓が黒いの。」
- Resident_B is dry and observational:
  - `dialogue.encounter.present_resident_b.line_2`: 「柱、棚、床板。壊れたものほど、日付を持っている。」

Assessment:

- No emoji, meme wording, industry slang, or over-explaining.
- The Japanese text avoids gendered Niro first-person pronouns. English uses first-person `I`, which is gender-neutral in context.
- The tone is quiet and atmosphere-led, aligned with the VS requirement for silence, decay, and restraint.
- `君` in Resident_B line 4 is not gendered and suits an older / detached speaker, though it does create a slightly instructive tone.

Improvement proposal:

- No immediate change required. If Resident_B should feel less mentor-like in Stage 4, line 4 can be softened from 「君も」 to a less direct construction such as 「通り過ぎるなら」, but this is polish rather than a defect.

## 4. ja-JP / en Alignment

**Verdict: ⚠️**

Evidence:

- Strong alignment:
  - `dialogue.niro.intro.line_3`: 「返せるものがあるなら、返したい。」 / "If something can be returned, I want to return it."
  - `dialogue.encounter.past_resident_a.line_3`: 「近づくなら、足音を小さくして。あそこ、まだ聞いてる気がする。」 / "If you go closer, keep your steps quiet. It still feels like that place is listening."
- Minor nuance drift:
  - `dialogue.encounter.present_resident_b.line_4`: 「君も、通り過ぎるなら、足元を見ておくといい。」 / "If you are passing through, keep your eyes on the ground."
  - `dialogue.speaker.resident_b`: 「記録者」 / "Recorder"

Assessment:

- Most lines are not mechanical translations; the English reads naturally while preserving the same image.
- Resident_B line 4 loses some of the Japanese "look at traces underfoot" texture and can read as a generic warning in English.
- `Recorder` is accurate but slightly object-like / mechanical in English. For a seated ruins / library figure, `Record Keeper`, `Chronicler`, or `Archivist` may better preserve the role tone, depending on final NPC identity.

Improvement proposals:

- Consider changing Resident_B line 4 EN to "If you pass through, watch what remains underfoot."
- Consider changing the Resident_B speaker label EN from `Recorder` to `Record Keeper` or `Chronicler` if the Stage 4 tone pass wants a more human role label.

## 5. Key Naming Rules

**Verdict: ⚠️**

Evidence:

- Runtime StringTable keys are consistent and match the requested final G3 shape:
  - `dialogue.niro.intro.line_1` through `dialogue.niro.intro.line_4`
  - `dialogue.niro.past_portal.line_1` through `dialogue.niro.past_portal.line_2`
  - `dialogue.encounter.past_resident_a.line_1` through `dialogue.encounter.past_resident_a.line_3`
  - `dialogue.encounter.present_resident_b.line_1` through `dialogue.encounter.present_resident_b.line_4`
- DialogueAsset instances point to those keys:
  - `Assets/ScriptableObjects/Dialogues/Niro_Intro.asset`
  - `Assets/ScriptableObjects/Dialogues/Niro_PastPortal.asset`
  - `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`
  - `Assets/ScriptableObjects/Dialogues/Resident_B_Idle.asset`
- Runtime placeholder references are absent from `Assets`.
- Documentation caveat outside this task's edit scope: `docs/api/dialogue_localization.md` still contains stale placeholder examples:
  - `dialogue.placeholder.resident_a.greet`
  - `dialogue.placeholder.resident_b.idle`
  - `new LocalizedString("Anemora_Strings", "dialogue.placeholder.resident_a.greet")`

Assessment:

- Runtime data and tests have completed the key migration.
- The contributor-facing API doc is now stale and can mislead new dialogue authors into creating `dialogue.placeholder.*` keys again.

Improvement proposal:

- Create a follow-up doc-only task to revise `docs/api/dialogue_localization.md` from placeholder-era examples to final G3 examples using `dialogue.niro.*` and `dialogue.encounter.*`.

## Proposed Follow-Up List

1. Stage 4 copy polish: add one subtle non-terminological true-cause hint line or object-response line.
2. Stage 4 EN polish: revise Resident_B line 4 EN toward "watch what remains underfoot."
3. Stage 4 EN label polish: decide whether `Recorder` should become `Record Keeper`, `Chronicler`, or `Archivist`.
4. Doc-only cleanup: update `docs/api/dialogue_localization.md` stale placeholder examples.

## Final Judgment

G3 final dialogue v0 is shippable for the current VS draft. It satisfies the hard constraints: no forbidden player-facing design terms, no family / acquaintance insertion, neutral Niro wording, and final runtime key migration. The main remaining issue is not correctness but signal strength: the deeper true-cause structure is present only as atmosphere, so Stage 4 should decide whether to strengthen it with one restrained line or environmental response.
