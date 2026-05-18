# Dialogue v1 Polish Review Sheet

Date: 2026-05-07

Scope: review draft only. No StringTable, DialogueAsset, scene, or production code changes are included here.

Inputs reviewed:

- `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset`
- `Assets/Localization/StringTables/Anemora_Strings_en.asset`
- `Assets/ScriptableObjects/Dialogues/*.asset`
- `docs/api/dialogue_localization.md`
- `docs/devlog/2026-05-05_g3_final_dialogue_self_audit.md`
- `docs/STAGE4_PHASE0_TRIAGE.md` Dialogue v1 polish row

Review notes:

- Proposal columns are the in-game display candidates.
- Niro proposals keep a neutral 15-19 voice and avoid family or acquaintance anchors.
- Resident_A proposals move toward an ordinary young past-side resident, using daily activity and street life for contrast.
- Resident_B proposals keep a darker current-side record tone while avoiding direct system explanation.
- Proposal count: 17 rows total, including 3 speaker labels, 7 Niro lines, 3 Resident_A lines, and 4 Resident_B lines.

## Speaker Labels

| Key | Current JP | Current EN | v1 Proposal JP | v1 Proposal EN | Intent | Risk | User Decision |
|---|---|---|---|---|---|---|---|
| `dialogue.speaker.niro` | ニロ | Niro | ニロ | Niro | Keep protagonist label stable and neutral. | None expected. |  |
| `dialogue.speaker.resident_a` | 少女 | Girl | 若い住人 | Young Resident | Make the past-side speaker feel like an ordinary town resident instead of a gender-locked archetype. | Less distinctive at a glance; use `少女` if the visual design strongly reads that way. |  |
| `dialogue.speaker.resident_b` | 記録者 | Record Keeper | 記録係 | Record Keeper | Keep the recording role, but make the JP label slightly more human and less abstract. | `記録者` is more mysterious; `記録係` is plainer. |  |

## Niro

| Key | Current JP | Current EN | v1 Proposal JP | v1 Proposal EN | Intent | Risk | User Decision |
|---|---|---|---|---|---|---|---|
| `dialogue.niro.intro.line_1` | アンテラの朝は、音より先に埃が動く。 | In Antela, dust moves before sound does. | アンテラの朝は、音より先に埃が揺れる。 | In Antela, dust stirs before the first sound. | Keep the quiet opening image, with a softer JP verb and more natural EN rhythm. | Minimal; current line is already strong. |  |
| `dialogue.niro.intro.line_2` | 誰かが残したはずの道なのに、歩くたび、少しずつ遠くなる。 | These streets were left by someone, but each step makes them feel farther away. | 残されている道なのに、歩くほど知らない場所になる。 | The road is still here, yet each step makes it less familiar. | Preserve dislocation without implying a known person or relationship. | Slightly less explicit about a prior owner or maker. |  |
| `dialogue.niro.intro.line_3` | 返せるものがあるなら、返したい。 | If something can be returned, I want to return it. | 戻せるものがあるなら、ここに戻したい。 | If anything can be restored, I want it back here. | Make the motivation more anchored to place while keeping Niro restrained. | EN `restored` may sound more active than the JP. |  |
| `dialogue.niro.intro.line_4` | 防げる終わりがあるなら、まだ間に合うと思いたい。 | If an ending can be held back, I want to believe there is still time. | 止められる終わりなら、まだ間に合うと信じたい。 | If this ending can be stopped, I want to believe there is still time. | Tighten the sentence and keep the vertical-slice goal legible. | Slightly more direct and heroic than current. |  |
| `dialogue.niro.intro.line_5` | ここを歩くたびに、何かが少しだけ薄れていく気がする。 | Each time I pass through here, something seems to fade a little. | ここを見つめるほど、残っていたものまで遠くなる。 | The longer I look, even what remains feels farther away. | Strengthen the subtle cause-and-effect unease without naming the rule. | Stronger hint may feel less ambiguous. |  |
| `dialogue.niro.past_portal.line_1` | 街が、息をしている。 | The town is breathing. | 街の音が、まだ近い。 | The sounds of the town are still close. | Shift the past glimpse toward immediate sensory life. | Current line is more iconic and poetic. |  |
| `dialogue.niro.past_portal.line_2` | 知らないはずの温度が、手のひらに残る。 | A warmth I should not know stays in my hand. | 知らないはずのぬくもりが、手のひらに残っている。 | A warmth I should not know is still in my palm. | Keep neutral first-person distance and make JP slightly softer. | Very small change; may not justify churn. |  |

## Resident_A

| Key | Current JP | Current EN | v1 Proposal JP | v1 Proposal EN | Intent | Risk | User Decision |
|---|---|---|---|---|---|---|---|
| `dialogue.encounter.past_resident_a.line_1` | あれ、見えてる？　向こうの大きな建物。 | Can you see it? The big building over there. | ねえ、向こうへ行くの？　今日は朝から人が多いよ。 | Are you headed that way? It has been busy since morning. | Reframe Resident_A as a living past-side street voice instead of a warning device. | Loses the direct building hook; scene route may need a separate visual cue. |  |
| `dialogue.encounter.past_resident_a.line_2` | 昨日まで灯りがついてたのに、今日は窓が黒いの。 | Its windows were lit yesterday. Today they are black. | 焼きたての匂いがして、店の声も近くまで聞こえるの。 | You can smell fresh bread, and the shopkeepers' voices carry all the way here. | Use ordinary morning life to contrast the current-side quiet. | Adds shops and bread as implied setting details; confirm they fit Antela. |  |
| `dialogue.encounter.past_resident_a.line_3` | 近づくなら、足音を小さくして。あそこ、まだ聞いてる気がする。 | If you go closer, keep your steps quiet. It still feels like that place is listening. | 急ぐなら、角を曲がって。荷車が通るから、足元だけ気をつけてね。 | If you are in a hurry, take the corner. Carts come through here, so mind your step. | Keep the speaker practical, local, and young without turning them into a dark explainer. | Adds carts and a corner route; may need adjustment to match layout. |  |

## Resident_B

| Key | Current JP | Current EN | v1 Proposal JP | v1 Proposal EN | Intent | Risk | User Decision |
|---|---|---|---|---|---|---|---|
| `dialogue.encounter.present_resident_b.line_1` | ここでは、崩れた順番だけがまだ残っている。 | Here, only the order of collapse has stayed intact. | ここでは、先に壊れたものほど静かに残る。 | Here, the first things broken are the quietest to remain. | Keep the current-side record tone, but reduce exposition. | More poetic and less precise than current. |  |
| `dialogue.encounter.present_resident_b.line_2` | 柱、棚、床板。壊れたものほど、日付を持っている。 | Pillars, shelves, floorboards. The more broken a thing is, the clearer its date becomes. | 柱、棚、床板。割れ目のひとつひとつに、日付が残っている。 | Pillars, shelves, floorboards. Every crack still keeps a date. | Preserve material detail and make the record-keeping image easier to read. | `Every crack` may overstate the precision. |  |
| `dialogue.encounter.present_resident_b.line_3` | 誰が来たかは記録しない。何が減ったかだけを書いている。 | I do not record who comes here. Only what has become less. | 誰が来たかではなく、来たあとに何が減ったかを書いている。 | I write down what is missing after someone comes, not who they were. | Strengthen the unsettling pattern while staying in character. | More causal than current; approve only if that hint is wanted in v1. |  |
| `dialogue.encounter.present_resident_b.line_4` | 君も、通り過ぎるなら、足元を見ておくといい。 | If you pass through, watch what remains underfoot. | 通るなら、足元に残った形だけ見ていくといい。 | If you pass through, note the shapes left underfoot. | Keep the underfoot warning, but make it less mentor-like than addressing Niro directly. | Less personal; current `君も` creates a stronger encounter beat. |  |

## Adoption Notes

If approved, apply selected rows to the existing keys in both locale tables:

- `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset`
- `Assets/Localization/StringTables/Anemora_Strings_en.asset`

Expected update shape:

- If the same keys and turn counts are retained, only the two StringTable locale values need updates.
- If any line is added, removed, split, or reordered, update the matching `Assets/ScriptableObjects/Dialogues/*.asset` turn references as part of that later implementation task.
- Prefer the Unity Localization editor for StringTable edits so shared ids and locale rows stay aligned.
- After adoption, run dialogue localization checks and any screenshot capture used for readability review.
