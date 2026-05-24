# Review workflow

`docs/review/` は **viewer (https://anemora-viewer.pages.dev/) のためにキュレーションしたレビュー対象画像**を入れる場所。Codex の作業ログ (`docs/devlog/screenshots/`) とは役割が違う (並存)。

このディレクトリの規律は **全セッション (Codex / Claude 含む)** が守る。違反は PR check (GitHub Actions) で fail する。

---

## 1. ディレクトリ命名

```
docs/review/<YYYY-MM-DDTHH-MM>/
```

- ISO 8601 + URL safe (`:` を `-` に置換)
- タイムゾーン: **JST**
- 例: `docs/review/2026-05-24T23-51/`

## 2. サイクル粒度

**1 セッション = 1 サイクルディレクトリ**。Codex の作業 1 セッションでレビューに出したい画像があれば、そのタイムスタンプで 1 ディレクトリ作成し、まとめて入れる。

- 画像枚数: 上限なし、Codex 判断
- セッション中に複数まとめて push しても、サイクル内に追加する形 (同じディレクトリに足す)

## 3. devlog.txt 必須

各サイクルディレクトリには `devlog.txt` を必ず置く。

```text
# 任意のコメント行 (省略可)
docs/devlog/2026-05-24_chapter1_session_intro.md
```

- 最初の非空・非コメント (`#` で始まらない) 行 = そのサイクルの対応 devlog markdown へのリポ相対パス
- 存在しない `.md` を指していると PR check fail

## 4. 既存 `docs/devlog/screenshots/` との違い

| ディレクトリ | 役割 | 主担当 |
|---|---|---|
| `docs/devlog/screenshots/` | Codex 作業中の生ログ・revision 含む雑多 | Codex (今まで通り) |
| `docs/review/<iso>/` | レビュー対象としてキュレーションした selected 画像 | Codex (このルールを守る) |

両方に同じ画像があっても良い (片方は raw、片方は selected)。

## 5. viewer での見え方

`marvelousu/anemora-viewer` (Cloudflare Pages) の **Review タブ** が `docs/review/*` を自動で一覧表示する:

- Cycle title: `Cycle 2026-05-24 23:51 (JST)` (ISO を人間向け整形)
- 代表サムネ・画像数
- devlog アイコン (タップで該当 .md ページにジャンプ)
- ★ で Pin、Pinned は Home タブにも集約表示
- 横スワイプで前後サイクル切替

`docs/review/` への commit を push すると、viewer 側 build は webhook で起動し、数分後に反映される。

## 6. PR check (`.github/workflows/review-check.yml`)

PR 中に `docs/review/**` が変更されると以下を自動で validate:

| # | チェック | fail 時 |
|---|---|---|
| 1 | ディレクトリ名が `^docs/review/\d{4}-\d{2}-\d{2}T\d{2}-\d{2}$` に適合 | rename を要求 |
| 2 | ディレクトリに `devlog.txt` が存在 | devlog.txt 追加を要求 |
| 3 | `devlog.txt` の最初の非空非コメント行が実在する `docs/devlog/*.md` を指す | path 訂正を要求 |
| 4 | ディレクトリに画像 (.png/.jpg/.jpeg/.webp/.svg/.gif) が 1 枚以上ある | 画像追加 or ディレクトリ削除を要求 |

## 7. レビュー完了後

サイクルディレクトリは **immutable** として残置。レビューが終わっても削除しない (devlog と対の歴史記録)。
