# Code License Options for Stage 4 Entry

> Status: v0.1 draft (2026-05-05). This document is decision material only and is not legal advice.

## 1. Overview

This document organizes code license options for the Stage 4 entry decision. It is intended to help compare trade-offs before any GitHub Public release, Steam submission, itch.io release, or continued private development decision.

Scope is limited to Anemora's original code: C# scripts, editor tools, tests, and repository-side helper scripts where Anemora owns the copyright. Art, audio, fonts, generated meshes, TMP font atlas assets, and other non-code assets remain separate and are tracked through `docs/legal/asset_ledger.md` and `NOTICES.md`.

The current project default is All Rights Reserved, as stated in `NOTICES.md` section 6. Until a license is added, redistribution, commercial use, and derivative works of Anemora's original code require author permission.

Choosing no public license at Stage 4 entry is also a valid outcome. The decision can remain open if public source release, contribution acceptance, and asset licensing policy are not ready to be settled.

## 2. Anemora Context

- Development model: single contributor at the Stage 3 Day 1 point.
- Engine: Unity 6000.3.14f1. Unity Editor and Unity packages keep their own license terms; this document only considers Anemora-owned code.
- Possible release forms:
  - Steam commercial release, with price, demo, DLC, and Early Access status TBD.
  - GitHub Public source release or continued private repository.
  - itch.io paid or free release.
  - No public release, with the project remaining private.
- AI-generated or AI-assisted assets are tracked in `docs/legal/asset_ledger.md`. The code license does not decide rights for PixelLab sprites, Meshy models, AIVA/Suno audio, ElevenLabs/Stable Audio SFX, or manually finished asset derivatives.
- Third-party fonts remain under their own terms: Misaki Gothic under its free-use permission text, and Press Start 2P under SIL Open Font License 1.1.

## 3. License Category Options

### 3.1 Permissive Licenses

#### MIT License

- Overview: a short permissive license. Reuse, modification, distribution, and commercial use are allowed when copyright and license notices are preserved.
- Pros: compact text, widely recognized, low friction for users and companies, simple repository setup.
- Cons: does not require derivatives to publish source, does not include an explicit patent grant, and allows proprietary forks.
- Anemora fit notes: relevant if the code is intended to be easy to reuse or study with minimal downstream conditions. It leaves asset licensing and brand use to separate terms.

#### BSD-2-Clause License

- Overview: a permissive license similar to MIT, with copyright notice and disclaimer preservation.
- Pros: simple, short, familiar in open-source ecosystems, and low friction for redistribution.
- Cons: same broad proprietary-fork allowance as MIT, with no explicit patent grant.
- Anemora fit notes: similar to MIT if the project wants source reuse with minimal obligations and separate asset restrictions.

#### BSD-3-Clause License

- Overview: BSD-2-Clause plus a non-endorsement clause restricting use of contributor or project names to promote derived products without permission.
- Pros: keeps permissive reuse while adding a project-name endorsement boundary.
- Cons: still allows proprietary forks and does not add explicit patent terms.
- Anemora fit notes: relevant if the code may be reused but Anemora's name should not be used to imply approval of derived products.

#### Apache License 2.0

- Overview: a permissive license with explicit patent license language, notice handling, and contribution-related terms.
- Pros: explicit patent grant and termination terms, familiar to companies, and compatible with many public code workflows.
- Cons: longer and more procedural than MIT/BSD, with NOTICE handling and file/header practices to maintain.
- Anemora fit notes: relevant if patent clarity or contributor patent grant language is considered important for public code.

### 3.2 Copyleft Licenses

#### GPL-3.0

- Overview: a strong copyleft license. Distributed modified versions must generally be licensed under GPL-compatible terms and provide corresponding source.
- Pros: preserves source availability for distributed derivatives and limits proprietary forks of covered code.
- Cons: can complicate commercial adoption, proprietary distribution, and interactions with engine/runtime packaging; compatibility review becomes more important.
- Anemora fit notes: relevant if the project wants derivative code to remain open under reciprocal terms. Steam and Unity packaging implications would need careful review before adoption.

#### AGPL-3.0

- Overview: GPL-3.0 with an added network interaction source-disclosure condition.
- Pros: addresses cases where modified software is offered over a network without distributing binaries.
- Cons: heavier compliance burden than GPL-3.0 and often a barrier for companies or service operators.
- Anemora fit notes: Anemora is currently a client-side game, so the network-use condition may have limited practical benefit unless future server-hosted systems become central.

#### LGPL-3.0

- Overview: a weaker copyleft license designed primarily for libraries, allowing applications to link to the library under certain conditions.
- Pros: useful when the licensed work is a reusable library and the application should be able to stay under another license.
- Cons: less natural for a complete Unity game application; separating library boundaries and relinking rights can be complex.
- Anemora fit notes: relevant mainly if a standalone Anemora library is split out. It is less directly aligned with licensing the whole game codebase.

### 3.3 Source-Available Licenses

#### Business Source License 1.1

- Overview: source-available license text with a change date and change license. Before the change date, production or commercial use can be restricted by additional terms.
- Pros: allows public source visibility while retaining a commercial protection period, then converts to an open-source license later.
- Cons: each use requires clear parameters, adoption is narrower than common OSI-approved licenses, and legal review is useful before relying on custom usage grants.
- Anemora fit notes: relevant if the project wants public source for transparency or review while preserving a time-limited commercial window.

#### Server Side Public License 1.0

- Overview: a source-available license based on GPL concepts with strong service-provider obligations for software offered as a service.
- Pros: focuses on preventing service operators from offering the software without releasing service-side source obligations.
- Cons: oriented toward server/SaaS products, can be difficult for downstream users to accept, and is not listed as an OSI-approved license.
- Anemora fit notes: Anemora is not currently a server-side SaaS product, so the main SSPL mechanism has limited direct relevance.

#### Functional Source License / Fair Source License

- Overview: a fair-source family where current versions are source-available under usage restrictions and convert to Apache 2.0 or MIT after a stated delay.
- Pros: combines source availability, delayed permissive release, and clearer standardized text than many custom licenses.
- Cons: newer and less familiar than MIT/Apache/GPL; exact version and conversion target must be checked before adoption.
- Anemora fit notes: relevant if the project wants current-source visibility with delayed permissive release rather than immediate open-source licensing.

### 3.4 Proprietary / All Rights Reserved

#### All Rights Reserved

- Overview: no public copyright license is granted beyond permissions explicitly stated elsewhere.
- Pros: retains maximum control over commercial use, redistribution, derivative works, and relicensing decisions.
- Cons: a GitHub Public repository would be source-visible but not generally reusable; outside contribution becomes legally harder unless contribution terms are defined.
- Anemora fit notes: relevant if the Stage 4 goal is transparency, backup, or portfolio visibility without allowing reuse or derivative works.

### 3.5 Custom / Hybrid

#### Anemora-Specific Custom License

- Overview: project-specific terms can allow or restrict particular uses, such as non-commercial mods, commercial approval requirements, or derivative-game limits.
- Pros: can express policy boundaries that standard licenses do not cover.
- Cons: no standard SPDX identifier, low familiarity, higher review burden for users, and professional legal drafting may be needed.
- Anemora fit notes: relevant only if standard licenses do not express the desired fork, mod, commercial-use, or source-visibility policy.

## 4. Decision Factors

### 4.1 Commercial Intent

- Steam commercial release: code and asset licensing should be checked against the intended revenue model, build distribution, mod policy, and repository visibility.
- Free release: license choice mainly affects reuse, derivatives, contribution, and public trust.
- Private development: leaving the code unlicensed can reduce decision surface until public release is clearer.

### 4.2 Community Contribution Willingness

- Accepting issues only: a public repository can keep code closed for reuse while still accepting bug reports.
- Accepting pull requests: contributor license handling should be defined, either through the project license alone, a Developer Certificate of Origin, a Contributor License Agreement, or explicit contribution terms.
- Not accepting contributions: All Rights Reserved or source-available terms can be easier to reason about, but should be stated clearly.

### 4.3 Derivative / Fork Policy

- Derivatives allowed with few conditions: permissive licenses such as MIT, BSD, or Apache 2.0.
- Derivatives allowed only under reciprocal source terms: copyleft licenses such as GPL-3.0.
- Derivatives restricted or prohibited: proprietary, source-available, or custom terms.

### 4.4 Patent Considerations

- Explicit patent language: Apache 2.0 and GPL-family licenses include patent-related terms.
- Minimal patent language: MIT and BSD keep the license simpler but do not provide the same explicit patent grant structure.
- Current project relevance: if Anemora adds patent-sensitive systems or outside contributors, this factor becomes more important.

### 4.5 Asset License Alignment

- Code and assets can use different licenses, but the repository should explain that separation clearly.
- AI-generated assets may be commercially usable under paid-plan terms, but that does not automatically make them open-source assets.
- If GPL-family code licensing is considered, build packaging and asset interaction should be reviewed case by case rather than assuming every asset follows the code license.

## 5. Comparison Matrix

| License / Option | Commercial use by others | Derivatives and forks | Same-license requirement | Patent terms | Adoption / operational familiarity | Anemora context memo |
|---|---|---|---|---|---|---|
| MIT | Allowed under notice preservation | Allowed, including proprietary forks | No reciprocal requirement | No explicit patent grant | Very familiar and short | Keeps code reuse simple; asset restrictions need separate terms |
| BSD-2-Clause | Allowed under notice preservation | Allowed, including proprietary forks | No reciprocal requirement | No explicit patent grant | Familiar and short | Similar operational shape to MIT |
| BSD-3-Clause | Allowed under notice preservation and non-endorsement | Allowed, including proprietary forks | No reciprocal requirement | No explicit patent grant | Familiar and short | Adds project-name endorsement boundary |
| Apache 2.0 | Allowed under license and NOTICE conditions | Allowed, including proprietary forks | No reciprocal requirement | Explicit patent grant and termination language | Familiar, especially for company use | Adds patent clarity and more license-management procedure |
| GPL-3.0 | Allowed, including paid distribution, if license obligations are met | Allowed under GPL-compatible reciprocal terms | Distributed derivatives generally stay under GPL-compatible terms | Includes patent-related terms | Familiar, but higher compliance surface | Relevant for reciprocal-source policy; Unity/Steam packaging review needed |
| AGPL-3.0 | Allowed if license and network-source obligations are met | Allowed under AGPL-compatible reciprocal terms | Distributed and network-used derivatives carry reciprocal obligations | Includes patent-related terms | Familiar in server software, less common for games | Network clause has limited relevance unless server-side Anemora systems emerge |
| LGPL-3.0 | Allowed with library-linking obligations | Allowed, usually centered on a library boundary | Modified library code remains under LGPL-compatible terms | Includes patent-related terms | Familiar for libraries | More relevant to a separated library than the full game application |
| BSL 1.1 | Depends on the license parameters before the change date | Usually source-visible with restricted production/commercial use until conversion | Converts later to the stated change license | Depends on selected change license and terms | Less common than OSI-approved licenses | Fits a public-source-with-commercial-window model if parameters are drafted clearly |
| SSPL 1.0 | Allowed only with strong service-side source obligations | Allowed under SSPL obligations | Strong reciprocal obligations for service offering | Based on GPL-style terms | Specialized and not OSI-approved | Mostly aimed at server/SaaS use rather than a client game |
| Functional Source License / Fair Source License | Allowed within current-use restrictions, then under MIT or Apache 2.0 after delay | Source-visible with restricted harmful competitive use before conversion | Converts to MIT or Apache 2.0 after the stated delay | Depends on FSL variant and conversion license | Newer and less familiar | Fits delayed permissive release if current-source visibility is desired |
| All Rights Reserved | Not allowed without permission | Not allowed without permission | No public reuse grant | No public patent grant | Legally familiar default, less open-collaboration friendly | Fits source visibility without reuse if GitHub Public is used as portfolio/transparency |
| Custom / Hybrid | Defined by custom terms | Defined by custom terms | Defined by custom terms | Defined by custom terms | Lowest familiarity unless reviewed and documented | Fits special policy goals but increases review and explanation burden |

## 6. No Recommendation; Decision Axes Only

This document does not select a license for Anemora. The Stage 4 decision depends on trade-offs that are not purely technical.

The user decision should be made against these axes:

1. Publication form: Steam commercial, GitHub Public, itch.io, or private.
2. Contribution expectation: issues only, pull requests accepted, or no public contribution.
3. Derivative policy: permissive reuse, reciprocal source, time-delayed source availability, or restricted reuse.
4. Patent concern: explicit patent terms needed or not needed at this stage.
5. Legal review: whether professional legal advice is available before public release.

No single option is treated here as the default answer. Keeping the current All Rights Reserved status is also a possible Stage 4 outcome if the project is not ready to grant public reuse rights.

## 7. Reference Resources

- Choose a License: https://choosealicense.com/
- Open Source Initiative license list: https://opensource.org/licenses
- SPDX License List and identifiers: https://spdx.org/licenses/
- TLDRLegal plain-language license summaries: https://www.tldrlegal.com/
- MIT License SPDX: https://spdx.org/licenses/MIT.html
- BSD-2-Clause SPDX: https://spdx.org/licenses/BSD-2-Clause.html
- BSD-3-Clause SPDX: https://spdx.org/licenses/BSD-3-Clause.html
- Apache License 2.0 SPDX: https://spdx.org/licenses/Apache-2.0.html
- GPL-3.0 / AGPL-3.0 / LGPL-3.0 GNU license texts: https://www.gnu.org/licenses/
- Business Source License 1.1: https://mariadb.com/bsl11/
- Server Side Public License: https://www.mongodb.com/legal/licensing/server-side-public-license
- Functional Source License: https://fsl.software/

This document is not legal advice. Before adopting a public license, especially for a commercial game release, professional legal advice should be considered.

## 8. Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点で、Stage 4 入口の code license 判断材料を整理 |
