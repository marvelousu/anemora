# R2 ephemeral-image pipeline (review / devlog screenshots)

Review and devlog images are **not** committed to git. They live in a Cloudflare
R2 bucket (with a date-based lifecycle TTL) plus a never-pruned local archive on
Tom's machine. The repo stays small; the viewer still shows recent imagery.
See `../../../anemora-repo-hygiene-cicd-plan.md` for the rationale (decision: case
B; no Git LFS).

## Viewer propagation rule (2026-06-14)

R2 upload alone does not update the live viewer. The viewer imports R2 manifests
during its Cloudflare Pages build. After running `tools/r2/r2-upload-review.ps1`,
also push the Anemora branch that owns the review/devlog cycle so the Anemora
push webhook triggers an `anemora-viewer` rebuild.

The viewer tracks active `work/*` and `wip/*` branches. The R2 slug is the branch
name with `work/` removed and non-alphanumeric separators converted to `-`, so
`wip/hd2d-point15-recovery-20260612` maps to:

```text
wip-hd2d-point15-recovery-20260612
```

Verify propagation by checking:

```powershell
Invoke-WebRequest -Method Head `
  https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/manifests/<slug>.json
Invoke-WebRequest -Method Head `
  https://anemora-viewer.pages.dev/<slug>/review
```

## Provisioned (2026-05-29)

Account `Maro6052@gmail.com's Account` (ID `fefd0ce0171bfcedbfc4e244876be220`):

- Bucket: **`anemora-review`**
- Public base URL (**PUBLIC_R2_BASE**): **`https://pub-d14764d639a647339a6b0d81de923abf.r2.dev`**
- Lifecycle TTL (45 days): prefixes `tree/`, `review/`, `devlog/`. `manifests/` never expires.
  (`review/` and `devlog/` only hold leftovers from an early Windows upload attempt and
  will self-expire; the live layout is `tree/`.)

## Bucket layout (authoritative)

```
tree/<slug>/docs/review/<ts>/<...>             # file bytes, full git path preserved
tree/<slug>/docs/devlog/screenshots/<...>
manifests/<slug>.json                          # JSON array of "docs/..." relative paths
```

`<slug>` = the work branch name minus `work/`, non-alphanumerics -> `-` (matches the
viewer's `slugify()`). The viewer reads `manifests/<slug>.json`, then fetches each
listed path from `tree/<slug>/<path>` (see anemora-viewer `scripts/setup-r2-images.mjs`).

## Backfill of existing imagery -> use the GitHub Action (Linux)

Bulk backfill runs in CI, **not** on Windows: Linux has no MAX_PATH limit (the deep
`docs/devlog/screenshots/...` paths broke `tar`/`Copy-Item` locally) and rclone uploads
in parallel. Workflow: `.github/workflows/r2-mirror-review.yml`.

One-time setup (needs Tom): create an **R2 S3 API token** and add it as repo secrets.

1. Cloudflare dashboard -> R2 -> **Manage R2 API Tokens** -> **Create API Token**
   - Permission: **Object Read & Write** (Admin R&W also works)
   - Apply to: bucket `anemora-review` (or all buckets)
   - Create -> copy the **Access Key ID** and **Secret Access Key**
2. GitHub repo `marvelousu/anemora` -> Settings -> Secrets and variables -> Actions -> New repository secret:
   - `R2_ACCESS_KEY_ID` = <Access Key ID>
   - `R2_SECRET_ACCESS_KEY` = <Secret Access Key>
3. Run the backfill: Actions tab -> **r2-mirror-review** -> Run workflow
   (leave `branches` empty to mirror all work/* active within 30 days, or pass specific ones).

The action also runs automatically on pushes to `work/**` that touch the imagery
paths, keeping R2 in sync while the images still live in git (i.e. before Phase D).

## Ongoing per-cycle upload (after Phase D, images no longer in git)

The AI review loop uploads each new cycle directly to R2 (small, shallow paths) via
`tools/r2/r2-upload-review.ps1`, which writes to `tree/<slug>/...` and unions the new
paths into `manifests/<slug>.json`:

```powershell
tools\r2\r2-upload-review.ps1 -CycleDir docs/review/2026-05-30T10-00 -Branch work/<branch>
```

Do **not** `git add` the images. The pre-commit guard (`tools/githooks`) blocks
`docs/review/` and `docs/devlog/screenshots/` from ever being committed.

## Auth / notes

- `wrangler` (object put/get, lifecycle) reads `CLOUDFLARE_API_TOKEN`
  (an Account "Workers R2 Storage: Edit" token; stored locally in `~/.cf_token`).
  `r2-upload-review.ps1` auto-loads `~/.cf_token` into the env when the var is unset,
  so the AI review loop can upload unattended (no interactive `wrangler login`).
- The GitHub Action uses the **S3** API (rclone) with `R2_ACCESS_KEY_ID` / `R2_SECRET_ACCESS_KEY`.
- IMPORTANT: every `wrangler r2 object put/get/delete` MUST pass `--remote`, or wrangler
  4.x targets a *local* simulated bucket. The scripts already include `--remote`.
- `tools/r2/r2-backfill-review.ps1` (Windows tar/Copy-Item) is **superseded** by the
  GitHub Action for bulk backfill; it is unreliable for deep paths and kept only for
  reference.
- The bucket is public (r2.dev), so the viewer needs no R2 secret at build time —
  only `PUBLIC_R2_BASE` set in the Cloudflare Pages project.
