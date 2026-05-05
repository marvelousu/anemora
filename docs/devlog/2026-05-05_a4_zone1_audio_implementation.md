# A4 Zone1 Audio Implementation (Retroactive)

Date: 2026-05-05
Status: Retroactive (本 devlog は task 完了後に orchestrator が memory + commit + handover から逆引きで起草)

## 1. スコープ

A4 Codex セッションが Zone1 の Audio (BGM + SFX 30 種 + Controller + Scene wiring + 各種 trigger hook) を実装。Stage 3 §H Audio = 死守ライン (`docs/VS_SCOPE.md`) を満たす。

## 2. 実施内容

| commit | 内容 |
| --- | --- |
| `6809c4b` | Add Zone1 audio implementation (BGM + SFX + controller) |
| `b9daccb` | Add Zone1 audio PlayMode test file |

### 2.1 Asset 追加

| Asset | 内容 |
| --- | --- |
| `Assets/Audio/Music/Zone1_Ambient.ogg` | BGM (Suno DustlightPiano_B + Studio One 仕上げ、3:04.84 / -18 LUFS / -4.5 dBTP / 48 kHz stereo) |
| `Assets/Audio/SFX/Zone1/environment/*.ogg` × 6 | env (wind / silence pad / distant_water 等) |
| `Assets/Audio/SFX/Zone1/footstep/*.ogg` × 12 | 足音 (地面 type 別) |
| `Assets/Audio/SFX/Zone1/timeframe/*.ogg` × 6 | 時の窓 (wheel open/close、portal open、flip 等) |
| `Assets/Audio/SFX/Zone1/npc/*.ogg` × 3 | NPC greeting / ack / departure |
| `Assets/Audio/SFX/Zone1/ui/*.ogg` × 3 | symbol select / hover / button click |
| Compatibility 30 件 (`Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/`) | primary と byte-for-byte 同一 (A4 audit `8871640` で発覚、Stage 4 削除候補) |
| 合計 OGG | primary 30 + compatibility 30 + BGM 1 = **61 件** |
| 全件 .meta | 60 件 (audio) + BGM .meta + 関連 .meta、missing 0 件 |

### 2.2 Script 追加

| file | 内容 |
| --- | --- |
| `Assets/Scripts/Audio/Zone1AudioController.cs` | Zone1_Audio root に attach、BGM / wind / pad / OneShot source、SFX clip assign、`autoPlayOnStart=True` |
| `Assets/Editor/Zone1AudioSceneSetup.cs` | Editor menu / Batch から VerifyMainScene 実行可能、配線 audit |

### 2.3 Scene wiring (`Assets/Scenes/Anemora_Main.unity`)

- `Zone1_Audio` root 追加 + Zone1AudioController 4 source children:
  - Music_Source (Zone1_Ambient)
  - Wind_Ambience_Source (sfx_env_wind_loop_01)
  - Pad_Ambience_Source (sfx_env_silence_pad_01)
  - OneShot_Source (env / portal / NPC / UI 共有)
- 各 trigger 接続:
  - PrototypePlayerController に石足音インターバル再生
  - TimeFramePortalController に wheel open/close / portal open / portal flip
  - SymbolWheelController に symbol select / hover
  - NpcInteractable / DialogueDisplay に NPC greeting / ack / departure

### 2.4 Test 追加 (別 commit `b9daccb`)

| file | 内容 |
| --- | --- |
| `Assets/Tests/PlayMode/Zone1AudioWiringTests.cs` (+ .meta) | 2 test method、PlayMode +2 (graphics 有効 batchmode で 25/25 passed 確認) |

## 3. 検証

| 項目 | 結果 |
| --- | --- |
| Unity batchmode `Anemora.EditorTools.Zone1AudioSceneSetup.VerifyMainScene` | success |
| Audio count (primary Zone1 SFX) | 30 |
| Audio count (compatibility SFX) | 30 (byte-for-byte duplicate、後段 `8871640` で発覚) |
| total OGG | 61 (primary 30 + compatibility 30 + BGM 1) |
| missing .meta | 0 |
| C# / docs git diff --check | OK |
| secret scan (xi-api-key 等の API key) | 実値なし、ヘッダ名のみ検出 |
| PlayMode 25/25 | `b9daccb` audio test commit 後に確認 |

## 4. 関連 doc

- `docs/legal/asset_ledger.md` (A5 `50ab8c0` で SFX 30 行 11 columns 形式統一 review)
- `~/shared-context/memory/tool_elevenlabs_audio.md` (orchestrator memory)
- `docs/devlog/2026-05-05_audio_qa_audit.md` (A4 `6b3eb93`、loudness / loop seam audit)
- `docs/devlog/2026-05-05_audio_compat_duplicate_investigation.md` (A4 `8871640`、compat 30 件 = primary 30 件 byte-for-byte duplicate 発見)
- `docs/devlog/2026-05-05_audio_prompts_integration_check.md` (A2 `5a1a39b`)

## 5. caveats / 既知 issue

- ElevenLabs SFX v2 API quota blocker → SFX 待機 → 解消後一気に 30 種実装
- Compatibility SFX 30 件は legacy fallback (canonical Zone1/... への移行用)、Stage 4 で final verify → 削除候補 (A4 `8871640` 結論)
- BGM は AIVA + Suno マッシュアップ想定だったが Suno DustlightPiano_B 単体採用で完成度十分と判明 (`docs/asset_prompts/bgm_zone1_ambient.md` § 3.3)
- DAW Reaper 想定 → Studio One 訂正 (ADR-0005 v1.1 `3a29757`)
- 後段で発覚: 本 audio commit + audio test 2 件は origin/main へ並行 push されず一時 dirty 状態 (A3 G5 automated `c17d62f` で audio 無し build が生成された原因) → A4 push 救援 (`6809c4b`、本 commit と同) で解消

## 6. 次の task / 引継ぎ

- audio 入り Windows build 再生成 (A3 G5 audio rebuild `e6e3c61`)
- Audio QA audit (A4 `6b3eb93`、LUFS / loop seam / SFX statistics)
- Compat SFX duplicate investigation (A4 `8871640`、Stage 4 削除推奨)
- Stage 4 で SFX normalize (high true peak 15 件、`6b3eb93` 識別)
- 重要: 本 devlog 起草前は VS が actually playable な体になっていない catastrophic failure が user 起動で発覚 (`docs/devlog/2026-05-05_vs_playable_failure_orchestration_postmortem.md` `0c3660d`)、原因特定は A3 diagnostic dispatch 中
