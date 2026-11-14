# Aegis Adblocker — Technical Feasibility Research

**Document Status:** Research Completed — Prototype Validation Pending  
**Project:** Aegis Adblocker  
**Platform:** Windows 11  
**Desktop Technology:** C#, WPF, .NET 10  
**Repository:** https://github.com/FragkopoulosFragkiskos/Aegis-Adblocker  
**Related Document:** `docs/business-analysis.md`

---

## 1. Purpose

The purpose of this document is to evaluate the technical approaches available for developing Aegis Adblocker.

Aegis Adblocker aims to reduce unwanted advertisements and tracking across Windows 11, with particular attention to:

- Google Chrome
- Microsoft Edge
- Mozilla Firefox
- YouTube
- Other applications where technically feasible

The research evaluates browser extensions, DNS filtering, Windows background services, and advanced system-level filtering.

The findings will be used to define a realistic Minimum Viable Product (MVP) and avoid unnecessary technical complexity during the initial development phase.

This document distinguishes documented technical capabilities from assumptions that still require practical testing.

## 2. Research Method

The initial assessment is based on official browser documentation, Microsoft documentation, and an evaluation of the project's requirements.

The main evaluation criteria are:

- Advertising-blocking capabilities
- Browser compatibility
- Implementation complexity
- Performance
- Privacy and security
- Required permissions
- Maintenance requirements
- Suitability for a beginner-friendly C# project

The research does not include a completed prototype or measured performance benchmarks. Therefore, implementation decisions remain subject to validation.

## 3. Candidate Technical Approaches

Four approaches were considered:

1. DNS filtering
2. Browser extensions
3. Windows background service
4. Advanced system-level network filtering

These approaches solve different problems and should not be treated as interchangeable.

### 3.1 DNS Filtering

DNS filtering uses domain-resolution rules to prevent selected domains from resolving normally.

A filtering component can compare requested domain names against a blocklist and return an appropriate response for blocked domains.

**Advantages**

- Can potentially affect multiple applications using the configured DNS resolver.
- Does not require a separate extension for every browser.
- Can block known advertising and tracking domains.
- Can complement browser-level filtering.

**Limitations**

- DNS filtering operates on domain names rather than individual page elements.
- It cannot reliably distinguish advertisements from normal content when both use the same domain.
- It cannot remove an advertisement that has already been downloaded.
- It cannot hide empty advertising spaces or modify page layout by itself.
- Its effectiveness depends on DNS configuration, caching, application behavior, and the availability of alternative resolution paths.
- DNS over HTTPS (DoH) and other encrypted DNS configurations can affect how DNS-based filtering is applied.

**Assessment**

DNS filtering is a possible future component for broader system coverage. It should not be the only advertising-blocking mechanism.

Before implementing it, the project must decide how DNS requests will be handled, how changes will be reversed, and how the application will recover if filtering causes connectivity problems.

Reference: Microsoft Learn — DNS encryption using DNS over HTTPS  
https://learn.microsoft.com/en-us/windows-server/networking/dns/dns-encryption-dns-over-https

Reference: Google Public DNS — Frequently Asked Questions  
https://developers.google.com/speed/public-dns/faq

### 3.2 Browser Extensions

Browser extensions operate within the browser's extension system and can apply rules to network requests or modify supported page content.

Chrome and Edge support the `declarativeNetRequest` API, which allows extensions to block or modify network requests using declarative rules.

Firefox uses the WebExtensions platform. Although it shares many extension APIs with Chromium-based browsers, differences in API support and manifest behavior must be considered.

**Advantages**

- Direct access to supported browser extension capabilities.
- Can block matching network requests before they are completed.
- Can potentially hide supported advertising elements using content scripts and CSS.
- Can provide browser-specific controls and diagnostics.
- Avoids the need to build a custom network driver for browser-level filtering.

**Limitations**

- Requires browser-specific testing and potentially different configurations.
- API permissions and browser policies affect available functionality.
- Network filtering alone does not remove every visual advertisement.
- Content scripts cannot operate on every browser page or privileged browser interface.
- Browser updates may require compatibility changes.
- The desktop WPF application cannot automatically control all browser extension behavior merely by existing as a desktop application.

**Assessment**

Browser extensions are a strong candidate for browser-specific advertising filtering.

However, a browser extension is a separate technical component from the WPF desktop application. Their communication, installation, configuration, and update mechanisms must be designed explicitly.

Chrome and Edge should be evaluated first because they support the documented Chromium extension APIs. Firefox compatibility should be evaluated separately rather than assumed.

References:

Chrome Developers — `declarativeNetRequest` API  
https://developer.chrome.com/docs/extensions/reference/api/declarativeNetRequest

Microsoft Learn — Supported APIs for Microsoft Edge extensions  
https://learn.microsoft.com/en-us/microsoft-edge/extensions/developer-guide/api-support

Mozilla MDN — Browser extensions  
https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions

Mozilla MDN — Chrome incompatibilities  
https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Chrome_incompatibilities

Mozilla MDN — Intercept HTTP requests  
https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Intercept_HTTP_requests

### 3.3 Windows Background Service

A Windows Service is a background process that can run independently of the desktop user interface.

A service may be appropriate when a product requires persistent background functionality, automatic startup, or operations that must continue when its user interface is closed.

**Advantages**

- Supports long-running background operations.
- Can be configured to start automatically.
- Separates background processing from the desktop interface.
- May be useful for a future system-wide filtering component.

**Limitations**

- Requires additional installation and lifecycle management.
- Introduces service permissions and security considerations.
- Requires communication between the WPF application and the service.
- Creates additional troubleshooting and testing work.
- Does not itself provide advertising-blocking capabilities. A service still needs an effective filtering mechanism.

**Assessment**

A Windows Service is not currently justified for the first prototype.

The initial application should investigate whether the required functionality can be delivered without a persistent service. A service can be introduced later if a validated requirement demonstrates that it is necessary.

Reference: Microsoft Learn — Develop Windows service apps  
https://learn.microsoft.com/en-us/dotnet/framework/windows-services/

### 3.4 Advanced System-Level Network Filtering

Advanced system-level filtering could provide coverage beyond browser extensions, depending on the mechanism used.

Possible approaches may involve network filtering components, proxy-based processing, or operating-system networking facilities.

**Advantages**

- May provide broader application coverage than browser-only filtering.
- Could support centralized filtering rules.
- May become useful for a future version intended to cover more desktop applications.

**Limitations**

- Requires significantly more research and implementation.
- May require elevated permissions and additional installation steps.
- Incorrect filtering can disrupt network connectivity.
- HTTPS inspection introduces serious security, privacy, and certificate-management concerns.
- A custom filtering driver or similar low-level component would substantially increase project complexity.
- Broad application coverage cannot be assumed without validating the selected mechanism.

**Assessment**

Custom system-level filtering is outside the initial MVP.

It should only be reconsidered after the simpler approaches have been tested and their limitations are documented.

The project must not introduce HTTPS interception merely to increase the theoretical number of advertisements it can block.

## 4. YouTube Feasibility

YouTube requires separate investigation because its advertising behavior cannot be treated as equivalent to a typical third-party advertising domain.

An advertisement may be associated with requests, page elements, and playback behavior that change over time. A rule that works for one scenario may fail in another.

DNS filtering alone cannot reliably distinguish all advertisements from regular video content.

Browser extensions may provide additional capabilities, but complete and permanent YouTube ad removal cannot be guaranteed.

### 4.1 Required Test Scenarios

The prototype evaluation should include:

- Opening the YouTube homepage.
- Opening a video directly.
- Starting and restarting video playback.
- Testing available pre-roll and mid-roll advertising scenarios.
- Navigating between videos.
- Testing signed-in and signed-out sessions where practical.
- Refreshing the page and restarting the browser.
- Checking for playback failures and broken page elements.
- Repeating tests after relevant browser or extension updates.

The tests must respect YouTube's applicable terms and the policies of the services being tested.

### 4.2 Measurement

For each scenario, record:

- Browser and version.
- Date of the test.
- Whether an advertisement was observed.
- Whether the video played successfully.
- Whether page elements or controls were broken.
- Whether the behavior was reproducible.
- Any relevant error messages.

An advertisement not appearing in one playback session does not, by itself, prove that the blocking mechanism caused the result.

### 4.3 Acceptance Principle

YouTube support should be described as a best-effort objective.

The product must not claim that it blocks every YouTube advertisement, and the initial MVP must not depend on achieving complete YouTube blocking.

Reference: YouTube Help — Ads on YouTube  
https://support.google.com/youtube/answer/3181017?hl=en

## 5. Comparison of Approaches

| Criterion | DNS Filtering | Browser Extension | Windows Service | System-Level Filtering |
|---|---|---|---|---|
| Browser-specific filtering | Limited | Strong candidate | Depends on implementation | Depends on implementation |
| Potential coverage beyond browsers | Yes, with limitations | No, normally browser-specific | Depends on filtering mechanism | Potentially broader |
| Page-element hiding | No | Possible | No, not by itself | No, not by itself |
| Implementation complexity | Medium | Medium | Medium to high | High |
| Additional permissions | Depends on DNS configuration | Browser permissions | Installation and service permissions | Potentially elevated permissions |
| YouTube effectiveness | Limited and unreliable | Requires testing | Depends on the underlying mechanism | Not guaranteed |
| MVP suitability | Possible later component | Strong candidate | Not currently justified | Not recommended |

The ratings in this table are qualitative assessments, not benchmark results.

## 6. Recommended MVP Direction

The recommended direction is to begin with the simplest architecture that can demonstrate useful, measurable functionality.

### 6.1 Desktop Application

The initial desktop application will use:

- C#
- WPF
- .NET 10
- A simple, maintainable project structure

The desktop application should provide a user interface for protection status, configuration, and relevant diagnostics.

The desktop application alone will not be considered a working ad blocker until an actual filtering mechanism has been implemented and tested.

### 6.2 Filtering Prototype

The first technical prototype should investigate a small browser-extension-based filtering implementation.

The prototype should demonstrate that a defined set of test requests can be blocked using supported browser APIs.

The prototype should also test the effect of the selected rules on normal website functionality.

### 6.3 Browser Compatibility

Chrome and Edge should be evaluated first.

Firefox should receive a separate compatibility assessment because browser extension APIs and behavior can differ.

A shared codebase may be possible, but this must be established through testing.

### 6.4 DNS Filtering

DNS filtering should remain a candidate for a later phase.

Before adding it, the project must document its configuration requirements, privacy implications, failure handling, and interaction with encrypted DNS.

### 6.5 Windows Service

A Windows Service should not be created solely because the product is intended to block advertisements across a PC.

It should only be added when a concrete requirement cannot be adequately implemented by the simpler architecture.

## 7. Security and Privacy Requirements

The application should follow these principles:

- Use the minimum permissions required by each component.
- Avoid collecting browsing history unless a clearly justified feature requires it.
- Keep diagnostic information limited to what is needed for troubleshooting.
- Do not send browsing data to external servers by default.
- Explain any DNS or network configuration changes before applying them.
- Provide a clear way to disable the application's protection.
- Handle configuration errors without silently breaking normal connectivity.
- Do not introduce custom HTTPS certificate installation or traffic interception without a separate security assessment.

Security and privacy must be evaluated as part of implementation, not treated as optional future improvements.

## 8. Risks and Mitigations

| Risk | Impact | Mitigation |
|---|---|---|
| YouTube changes its advertising behavior | Blocking becomes less effective | Use repeatable tests and document limitations |
| Browser APIs differ | Inconsistent behavior between browsers | Test Chrome, Edge, and Firefox separately |
| Filtering blocks legitimate content | Websites stop working correctly | Test allowlisting and normal site functionality |
| DNS configuration conflicts | Applications may lose connectivity | Delay system changes and design recovery procedures |
| Excessive permissions | Security and user-trust risks | Apply least-privilege principles |
| Too much architectural complexity | Slow development and difficult debugging | Build a small prototype before expanding |
| Browser updates break compatibility | Existing rules or extensions stop working | Maintain compatibility tests and update documentation |

## 9. Proposed Experiments

The following experiments are required before finalizing the architecture.

### Experiment 1 — Browser Request Blocking

Create a minimal extension prototype using documented browser APIs.

Verify that a known test request is blocked and that unrelated requests continue to work.

### Experiment 2 — Page-Element Filtering

Evaluate whether content scripts and CSS can hide selected test elements without breaking the surrounding page.

### Experiment 3 — Browser Compatibility

Repeat the relevant tests in Chrome, Edge, and Firefox.

Record differences in permissions, manifest configuration, and behavior.

### Experiment 4 — YouTube Behavior

Execute the test scenarios defined in Section 4.

Record actual results rather than assuming that a rule or extension will work.

### Experiment 5 — DNS Filtering

Evaluate DNS filtering in a controlled environment before modifying the primary Windows network configuration.

Test normal domain resolution, blocked domains, encrypted DNS behavior, and recovery from configuration errors.

### Experiment 6 — Background Operation

Determine whether the prototype needs to continue filtering when the WPF window is closed.

Use the result to decide whether a background process or Windows Service is justified.

## 10. Decisions and Open Questions

The following decisions remain open until the relevant experiments are completed:

1. Should the first release include a browser extension, or should the desktop application initially serve as a configuration and monitoring interface?
2. How should the WPF application communicate with browser extensions?
3. Should DNS filtering be included in the MVP or deferred?
4. What minimum level of Firefox compatibility is required for the first release?
5. Which filter lists can be used, and what are their update and licensing requirements?
6. What functionality must remain active when the desktop window is closed?
7. Which YouTube test results are sufficient to consider the prototype useful?
8. What installation and update process will be required for the desktop application and any extensions?

These questions must be answered with implementation evidence where practical.

## 11. Technical Acceptance Criteria

This research document is considered complete when:

- The main technical approaches have been evaluated.
- Chrome, Edge, and Firefox compatibility differences are documented.
- DNS filtering limitations are documented.
- YouTube limitations and test scenarios are defined.
- The security implications of background services and system-level filtering are considered.
- A preliminary MVP direction is documented.
- The distinction between documented capabilities and unverified prototype behavior is clear.
- The sources used for the research are listed.
- The next experiments are defined.

Completion of this document does not mean that the proposed architecture has already been validated.

## 12. Conclusion

The initial research supports investigating browser extensions as the first filtering prototype, while retaining DNS filtering as a possible complementary component.

The WPF desktop application can provide the user interface, but it must be integrated with a real filtering mechanism to deliver advertising-blocking functionality.

A Windows Service and custom system-level filtering should be deferred until practical testing demonstrates a need for them.

YouTube remains a best-effort target with no guarantee of complete ad removal.

The next phase should validate the proposed direction through small, controlled experiments before the project commits to a final architecture.

## 13. References

1. Chrome Developers — `declarativeNetRequest` API  
   https://developer.chrome.com/docs/extensions/reference/api/declarativeNetRequest

2. Microsoft Learn — Supported APIs for Microsoft Edge extensions  
   https://learn.microsoft.com/en-us/microsoft-edge/extensions/developer-guide/api-support

3. Mozilla MDN — Browser extensions  
   https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions

4. Mozilla MDN — Chrome incompatibilities  
   https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Chrome_incompatibilities

5. Mozilla MDN — Intercept HTTP requests  
   https://developer.mozilla.org/en-US/docs/Mozilla/Add-ons/WebExtensions/Intercept_HTTP_requests

6. Microsoft Learn — DNS encryption using DNS over HTTPS  
   https://learn.microsoft.com/en-us/windows-server/networking/dns/dns-encryption-dns-over-https

7. Google Public DNS — Frequently Asked Questions  
   https://developers.google.com/speed/public-dns/faq

8. Microsoft Learn — Develop Windows service apps  
   https://learn.microsoft.com/en-us/dotnet/framework/windows-services/

9. YouTube Help — Ads on YouTube  
   https://support.google.com/youtube/answer/3181017?hl=en


## 14. Windows-Level Filtering Research

### 14.1 Objective

Investigate whether Aegis Adblocker can provide useful advertising-blocking functionality on Windows 11 without requiring browser extensions.

The investigation focuses on DNS filtering, Windows Filtering Platform (WFP), and the limitations of filtering encrypted browser traffic.

### 14.2 Windows Filtering Platform

Windows Filtering Platform is a Windows networking platform that allows applications to configure and apply network filtering rules at supported layers of the networking stack.

It supports filtering based on application identity at Application Layer Enforcement (ALE) layers and provides mechanisms for filtering network connections and traffic.

WFP does not automatically identify advertisements or distinguish advertising content from normal website content.

The application must define the filtering rules and determine which network connections should be permitted or blocked.

The initial investigation should prioritize existing user-mode WFP management functionality. A custom kernel-mode callout driver should not be developed unless a demonstrated requirement cannot be addressed by simpler mechanisms.

Official references:

- https://learn.microsoft.com/en-us/windows/win32/fwp/about-windows-filtering-platform
- https://learn.microsoft.com/en-us/windows/win32/fwp/application-layer-enforcement--ale-
- https://learn.microsoft.com/en-us/windows-hardware/drivers/network/callout-driver-programming-considerations

### 14.3 DNS Filtering Without Browser Extensions

DNS filtering is a candidate for cross-application domain blocking without browser extensions.

Its effectiveness depends on the availability of suitable domain blocklists, DNS configuration, caching, encrypted DNS behavior, and the domains used by advertisements.

DNS filtering cannot reliably remove advertisements that share infrastructure with legitimate content.

The prototype must not assume that configuring Windows DNS automatically controls every DNS request made by every browser or application.

Official references:

- https://learn.microsoft.com/en-us/windows-server/networking/dns/dns-encryption-dns-over-https
- https://support.microsoft.com/en-gb/windows/experience/connectivity-networking/essential-network-settings-and-tasks-in-windows

### 14.4 HTTPS and Shared Infrastructure Limitations

Modern browsers commonly communicate with websites over encrypted HTTPS connections.

Network-level filtering can block selected connections, but blocking a connection does not necessarily remove only the advertising content carried through it.

If advertising and legitimate content use the same domain or connection, blocking the connection may also disrupt legitimate content.

Inspecting encrypted application content would require additional mechanisms and a separate security and privacy assessment.

The initial prototype must not introduce HTTPS interception or custom certificate installation.

### 14.5 Browser Detection Versus Content Filtering

Aegis may be able to identify installed browsers and detect their running processes.

However, browser detection does not grant access to page elements, browser network requests, or the browser's internal advertising decisions.

Browser detection should therefore be treated as a separate feature from advertising filtering.

### 14.6 Initial Windows-Level Prototype

The proposed prototype should investigate:

1. Domain blocking in a controlled DNS filtering test.
2. Application-specific connection filtering using supported WFP functionality.
3. The permissions required to configure and remove filtering rules.
4. The effects of encrypted DNS and alternative network resolution paths.
5. The ability to disable protection and restore previous settings.
6. Whether normal browser navigation remains functional.
7. The limitations observed during YouTube playback tests.

No changes to the primary machine's network configuration should be made until a safe test and recovery procedure has been defined.

### 14.7 Preliminary Assessment

A Windows-level approach is technically feasible for certain forms of domain and connection filtering without browser extensions.

However, current research does not establish that Windows-level filtering alone can provide comprehensive cosmetic filtering or reliable removal of all advertisements across Chrome, Edge, Firefox, and YouTube.

The project should validate the simplest Windows-level approach before deciding whether browser extensions or more advanced filtering components are necessary.

### 14.8 Decision Status

**Decision: Pending prototype validation.**

The final architecture must be based on practical test results rather than the assumption that system-level filtering can replace browser-level filtering.

The next step is to implement a small, reversible proof of concept and document its results before committing to the production architecture.
