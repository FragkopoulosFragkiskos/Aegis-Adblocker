# Aegis Adblocker — Business Analysis Document

**Document status:** Draft — for review  
**Product:** Aegis Adblocker  
**Platform:** Windows 11  
**Primary technology:** C#, WPF, .NET 10  


## 1. Executive Summary

Aegis Adblocker is a proposed Windows desktop application intended to give users a centralized way to manage protection against online advertisements and tracking.

The product aims to combine a simple desktop interface with suitable filtering technologies, browser integration, customizable rules, and transparent protection status.

The initial focus is Windows 11, with particular attention to Google Chrome, Microsoft Edge, Mozilla Firefox, and the technical challenges associated with advertisements on YouTube.

The product will be developed incrementally. The initial version will prioritize a reliable and testable core rather than attempting to implement every advanced feature immediately.

Aegis must not claim that it blocks every advertisement. Blocking effectiveness depends on the browser, filtering method, filter lists, website behavior, and changes introduced by third-party services.

## 2. Business Background and Problem Statement

### 2.1 Current problem

Users may encounter advertisements, tracking requests, pop-ups, intrusive page elements, and unwanted network connections while browsing or using desktop applications.

Common problems include:

- Different browsers may require separate protection settings.
- Browser extensions may have different capabilities and limitations.
- DNS-based filtering can block requests to known advertising domains but cannot reliably remove every advertisement embedded in content.
- Some advertisements are delivered through infrastructure shared with legitimate content.
- Users may not understand whether protection is active or which filtering method is responsible.
- Overly aggressive rules can break websites or applications.
- Filter lists require maintenance as advertising systems change.

### 2.2 Problem statement

Users need a convenient and understandable way to manage ad-blocking protection on Windows, while retaining control over filtering behavior and minimizing unnecessary changes to their system.

Aegis will investigate whether a combination of desktop management, domain filtering, and browser-specific filtering can meet this need without introducing excessive complexity or unreliable system modifications.

## 3. Business Objectives

The initial business objectives are:

1. Provide a clear desktop interface for enabling and disabling supported protection features.
2. Establish a modular architecture that can support multiple filtering methods.
3. Investigate practical coverage across Chrome, Edge, and Firefox.
4. Measure blocking effectiveness using repeatable tests.
5. Minimize false positives and provide a way to diagnose broken websites.
6. Explain protection status and filtering limitations to users.
7. Avoid collecting personal browsing data unless a future feature explicitly requires it and the user has been informed.
8. Maintain a codebase that is understandable, testable, and suitable for an individual developer or a small team.
9. Provide documentation that explains requirements, architecture, decisions, tests, and known limitations.

## 4. Target Users and Stakeholders

### 4.1 Primary users

**Everyday Windows users**
- Want fewer intrusive advertisements.
- Prefer a simple enable/disable control.
- Need clear feedback when protection is unavailable.

**Privacy-conscious users**
- Want to reduce known tracking requests.
- Prefer transparent filtering rules.
- Want to understand what data the application processes.

**Technical users**
- May want custom blocklists, allowlists, logs, and troubleshooting tools.
- Need understandable error messages and control over filtering behavior.

### 4.2 Stakeholders

- End users
- Product owner/developer
- Browser extension maintainers, if applicable
- External filter-list maintainers
- Windows and browser platform providers

## 5. Product Scope

### 5.1 Proposed MVP

The Minimum Viable Product should demonstrate a small but reliable end-to-end workflow.

The proposed MVP includes:

- A desktop dashboard.
- A visible protection status.
- A basic domain-filtering capability.
- Loading and applying a selected filtering list.
- A user-controlled allowlist for exceptions.
- Basic event logging without recording unnecessary browsing history.
- Clear error handling.
- Automated tests for core filtering logic.
- Documentation of supported behavior and limitations.

The exact filtering mechanism must be selected after technical investigation. The dashboard must not claim that system-wide protection is active unless the underlying filtering component has confirmed that it is working.

### 5.2 Future scope

Potential future capabilities include:

- Browser extension integration.
- Separate filtering profiles.
- Filter-list update management.
- Protection statistics.
- Per-application rules.
- Advanced diagnostics.
- Startup behavior and background operation.
- A Windows service, if required by the selected architecture.
- More advanced cosmetic filtering.
- Additional browser support.

These capabilities are candidates, not commitments for the MVP.

### 5.3 Explicit exclusions for the initial release

- Guaranteed removal of every YouTube advertisement.
- Guaranteed blocking of every advertisement in every desktop application.
- Cloud accounts or synchronization.
- Remote collection of browsing activity.
- Parental controls.
- Enterprise policy management.
- A custom HTTPS interception engine before its security, compatibility, and maintenance costs have been assessed.

## 6. Functional Requirements

Requirements will receive unique identifiers so they can be linked to GitHub Issues and test cases.

### Protection management

**FR-001:** The application shall display the current protection status.

**FR-002:** The application shall allow users to enable or disable each supported protection feature.

**FR-003:** The application shall distinguish between a requested state and a confirmed operational state.

**FR-004:** The application shall explain when a protection feature cannot start or has encountered an error.

### Filtering

**FR-005:** The filtering engine shall evaluate a request or domain against configured blocking rules.

**FR-006:** The application shall support a documented initial blocklist format.

**FR-007:** The application shall support explicit allowlist exceptions.

**FR-008:** The application shall handle malformed rules without crashing.

**FR-009:** The application shall provide a defined way to refresh filtering rules.

**FR-010:** The filtering engine shall have automated tests covering blocked domains, allowed domains, invalid rules, and rule conflicts.

### User interface

**FR-011:** The dashboard shall display protection status and relevant errors.

**FR-012:** Users shall be able to access the main protection settings without navigating complex menus.

**FR-013:** The application shall provide clear messages when administrator privileges are required.

**FR-014:** The interface shall remain responsive during supported background operations.

### Logging and diagnostics

**FR-015:** The application shall record essential operational events and errors.

**FR-016:** Logs shall avoid storing full URLs, query parameters, or browsing history unless a clearly justified feature requires them.

**FR-017:** The application shall make filtering failures understandable to the user.

### Browser integration

**FR-018:** Chrome, Edge, and Firefox shall be evaluated independently for compatibility.

**FR-019:** Browser-specific functionality shall be isolated from the desktop application's core filtering logic.

**FR-020:** Any browser extension shall request only the permissions necessary for its functionality.

**FR-021:** Browser integration shall be considered complete only after testing on the supported browser versions.

### YouTube

**FR-022:** YouTube ad blocking shall be treated as a separate technical investigation and test area.

**FR-023:** Results shall distinguish between video advertisements, page advertisements, sponsored content, and other unwanted page elements.

**FR-024:** The application shall document failures and limitations rather than reporting guaranteed success.

## 7. Non-Functional Requirements

### Security

- System-level operations shall use the least privileges necessary.
- The application shall not disable security features to simplify filtering.
- Network and system configuration changes shall be explicit and recoverable.
- Browser extension permissions shall be minimized.
- Updates and downloaded filtering lists shall be handled with appropriate validation.

### Privacy

- Browsing data shall remain local by default.
- No analytics or remote telemetry shall be introduced without a documented decision.
- Logs shall be minimized and configurable where appropriate.
- The application shall document what information is processed.

### Reliability

- Invalid filter data shall not crash the application.
- Protection failures shall be reported accurately.
- Failed updates shall not silently replace a working configuration with invalid data.
- Disabling or uninstalling protection shall have a documented recovery path.

### Performance

- Filtering should introduce minimal additional latency.
- The interface should not freeze during rule loading or updates.
- Memory usage and rule-evaluation performance shall be measured rather than assumed.

### Maintainability

- Components shall have clear responsibilities.
- Methods and classes shall remain straightforward and readable.
- Automated tests shall cover important business logic.
- Architecture decisions and known limitations shall be documented.
- Unnecessary design patterns and abstractions shall be avoided.

### Usability

- Protection status shall be understandable to non-technical users.
- Errors shall include practical next steps.
- The interface shall distinguish blocked requests from confirmed blocked advertisements.

## 8. Competitive and Market Analysis

Existing products demonstrate that ad blocking can be implemented at different layers.

### Browser-based blockers

Browser extensions can filter network requests and remove or hide page elements. Their capabilities depend on browser APIs, permissions, and extension policies.

### System-level blockers

Products such as AdGuard for Windows combine a desktop interface with network-level and content-filtering capabilities. This demonstrates the potential value of centralized management, but also the additional engineering required for deeper system integration.

### DNS-based filtering

DNS filtering can block requests to known advertising and tracking domains. However, it cannot reliably remove advertisements delivered from the same domains as wanted content, nor can it hide arbitrary page elements by itself.

### Potential differentiation

Aegis should not attempt to compete solely on the number of blocked domains.

Potential areas of differentiation include:

- A straightforward interface.
- Transparent status and diagnostics.
- Clear documentation of filtering behavior.
- Privacy-first defaults.
- Simple rule management.
- Reliable recovery when protection is disabled or fails.
- An architecture that makes it easy to test and extend filtering components.

A more detailed competitor comparison should be completed before making claims about unique market advantages.

## 9. Technical Feasibility and Constraints

### 9.1 Desktop application

C#, WPF, and .NET 10 are suitable candidates for the Windows desktop interface and application logic.

They do not, by themselves, provide system-wide network filtering.

### 9.2 DNS/domain filtering

DNS filtering is a reasonable candidate for an initial filtering component because its responsibilities are comparatively limited.

Its coverage is also limited: a blocked domain may affect legitimate content, and advertisements served through shared domains may not be separable from wanted content.

### 9.3 Browser extensions

Browser extensions can provide filtering closer to the web page and its requests.

Chrome and other Chromium-based browsers impose restrictions on extension APIs. Manifest V3 uses declarative network request rules for important blocking scenarios, which affects how an extension can implement filtering.

Firefox and Chromium-based browsers must therefore be tested separately rather than treated as identical platforms.

### 9.4 YouTube

YouTube requires dedicated investigation because advertising delivery can change and may share infrastructure with legitimate video content.

The project should define a repeatable test matrix and record observed results. No architectural decision should assume that DNS filtering alone will solve this problem.

### 9.5 Deeper system-level filtering

A network driver, traffic interception, or HTTPS filtering would increase implementation complexity, privilege requirements, security risks, compatibility concerns, and maintenance costs.

These approaches shall not be selected for the MVP without a documented technical evaluation.

## 10. Key Risks

| Risk | Impact | Proposed mitigation |
|---|---|---|
| Incomplete ad blocking | High | Combine appropriate filtering layers and measure coverage |
| YouTube changes its delivery mechanisms | High | Maintain a separate test suite and document limitations |
| False positives | High | Allowlist, regression tests, and recovery procedures |
| Browser API restrictions | High | Evaluate each browser and its supported extension model |
| System configuration mistakes | High | Minimize privileges and provide rollback |
| Excessive project complexity | Medium | Deliver a small MVP and defer advanced features |
| Stale or malformed filter lists | Medium | Validate updates and preserve the last known working configuration |
| Unclear protection status | High | Report actual component health, not just user intent |

## 11. Success Metrics

The project should define measurable acceptance criteria before implementation.

Candidate metrics include:

- Percentage of supported test cases correctly blocked.
- Percentage of legitimate test cases incorrectly blocked.
- Filtering latency under a documented test workload.
- Application startup and rule-loading reliability.
- Successful recovery after disabling protection.
- Browser compatibility across the defined test matrix.
- Number of automated tests passing in continuous integration.
- Completeness of user-facing error messages and documentation.

Specific numerical targets shall be selected after establishing a baseline and defining the test environment.

## 12. Assumptions and Open Questions

- The initial supported operating system is Windows 11.
- The desktop application will use C#, WPF, and .NET 10.
- The product should prioritize local operation and privacy.
- Chrome, Edge, Firefox, and YouTube are important compatibility targets.
- The filtering architecture remains undecided.
- A browser extension may be required for capabilities that DNS filtering cannot provide.
- The need for a Windows service or network driver must be evaluated.
- The distribution model, signing, update mechanism, and installation experience remain open decisions.

## 13. Acceptance Criteria for Business Analysis

This document can be approved when:

- The target users and primary problems are defined.
- The initial product scope is explicit.
- Functional and non-functional requirements have unique identifiers.
- Technical risks and known limitations are documented.
- Success metrics have measurable definitions.
- Open questions have owners or planned investigation tasks.
- Requirements can be traced to future GitHub Issues and test cases.

## 14. Deliverables

The Business Analysis phase should produce:

1. This Business Analysis document.
2. A prioritized product scope and MVP definition.
3. A functional requirements specification.
4. A non-functional requirements specification.
5. A risk register.
6. A list of open technical questions.
7. A traceability plan connecting requirements, GitHub Issues, and tests.

The architecture and implementation plan shall be finalized only after the important open questions have been investigated and reviewed.
