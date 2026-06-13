# 2026-06-13 環境監査 — point15 ループの現状診断とレンダラ凍結提案

監査主体: Win Claude (Fable 5)。読み取り専用で実施 (稼働中の point15 ループとは非干渉)。
本ファイルは immutable 規律に従い新規追加。INDEX.md への登録はループ停止時の一括コミットに委ねる。

## 検出した問題 (重大度順)

1. **保全リスク (処置済み)**: 本 branch (`wip/hd2d-point15-recovery-20260612`) は origin 未push の547コミット+未コミット2週間分 (devlog 32本 untracked / modified 44) の状態だった。
   → 本日 branch を origin へ push。`work/chapter1-continuation-20260520` (head cbeedfe、どのリモートからも到達不能だった) を bundle から origin へ復元。夜間バックアップ (bundle+dirty.patch+untracked) を常設タスク化。
2. **文書漂流 (処置済み)**: STATUS.md=05-24 / INDEX.md=05-20 のまま実装は06-13。STATUS を現在地へ更新し、pre-push hook に STATUS 鮮度ガード (7日超で fail) を追加。
3. **polish 頭打ちの再演 (要判断)**: レビュー cycle 51〜125 は fog/alpha/帯のアブレーション探索 (本日だけで motion 141枚×17変種)。一方、全マップ俯瞰の実画確認では環境がブロックアウト水準 (植生=プリミティブ、地面=均一タイル、マップ端 void 露出、Zone1 アトラス512px)。**レンダラパラメータでは埋まらない素材ギャップ**であり、cycle125 でのレンダラ凍結を提案。
4. **authored file の堆積**: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` = 81,131行 / 1,903メソッド、実質実装は推定6〜10%。Cycle 1〜105 の死世代162メソッド、Refresh 重複約300回、参照ディレクトリの日付ハードコード。partial 分割は cycle-runner 契約に透過で実施可能。
5. **アセット規律**: 承認済み v57/v58 ジェネリック NPC が削除済み worktree と共に紛失した既往。v59 review_only 版が runtime 参照に混入し得る (バージョンロック検証なし)。530+ の cycle 命名マテリアルに dead 検出なし。

## 凍結提案の中身 (ループ停止時に適用)

- 現行レンダラ設定 (Renderer Feature 4種 / APV / air alpha 0.60) を契約スナップショット化し EditMode テストで固定 → cycle-runner の -RunTests 第0フェーズで常時検査
- 以後のアーティファクト追跡はアブレーション撮影ではなく Unity MCP の構造照会 + shotdiff (画素差分トリアージ) で行う
- エネルギーは環境アセット物量 (PolyHaven テクスチャ / meshy image-to-3d 植生・小物 / HDRI 空 / アトラス2K化) へ転換。検収は ValidateImportedAssetsBatch (新設) で機械化

適用物のステージング一式と手順は incoming ディレクトリ (リポ外、ループ停止時にコピー適用) に準備。

## 未決事項 (ユーザー判断待ち)

- ビジュアルターゲットの正: 設計文書「Tier 2 意図的選択」vs 目標「最高ティア (Tier 4)」(外部タイトル名は公開リポに出さない)
- `origin/wip/snapshot-repair-proof-20260603` (現行と平行分岐) の処遇
- cycle125 凍結の最終承認
