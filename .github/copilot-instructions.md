# GitHub Copilot Instructions for RimWriter Mod

## Mod Overview and Purpose
RimWriter is an unofficial update to Jecrell's mod, adding a variety of writing-related items and furniture to the RimWorld game. The mod aims to enhance the gameplay experience by introducing tablets, books, scrolls, libraries, and other writing-related items. It provides a framework that can be expanded upon for future projects and integrates well with other mods like Lord of the Rims and Call of Cthulhu.

## Key Features and Systems
### Items
- **Stone Tablets**: Basic writing surfaces.
- **Leather Scrolls**: Portable writing materials.
- **Journals**: Handwritten diaries.
- **Handwritten Books**: Books that can be written in.
- **Typed Books and Journals**: Digital books and journals.
- **Skillbooks**: Books that improve specific skills.

### Furniture
- **Stone Shelves**: Storage for tablets.
- **Scrollcases (tall and short)**: Storage for scrolls.
- **Bookcases (tall and short)**: Storage for books.
- **Writer's Tables**: Dedicated tables for writing.
- **Typewriter**: A device for typing.

### Features
- **Customized Text Generation**: Allows for unique and varied text on tablets, books, and journals.
- **Compatibility with Other Mods**: Works with Lord of the Rims and Call of Cthulhu. In Call of Cthulhu, writing reduces sanity loss over time. Future compatibility with storing Grimoires and Wizard textbooks inside bookcases/scrollcases.

## Coding Patterns and Conventions
RimWriter follows standard RimWorld modding conventions. Key aspects include:
- **Modding Framework**: Utilizes the RimWorld modding framework, which is based on XML and C#.
- **Harmony Patching**: Uses Harmony patches to modify vanilla RimWorld behavior where necessary.
- **XML Definitions**: Extensive use of XML files to define new items, jobs, research projects, and more.
- **C# Code**: Implements logic for new features, such as text generation and furniture interactions.

## XML Integration
RimWriter defines new items, jobs, research projects, and other game elements using XML files. These files are located in the `Defs` folder and include:
- **ConceptDefs**: Define new concepts.
- **JobDefs**: Define new jobs.
- **ResearchProjectDefs**: Define new research projects.
- **Rooms**: Define new room roles.
- **JoyGiverDefs**: Define new joy givers.

### Example XML Definition
xml
<ConceptDef>
  <defName>RimWriter_Concept</defName>
  <label>Writing</label>
  <description>This concept encompasses all writing-related items and furniture in RimWorld.</description>
</ConceptDef>


## Harmony Patching
RimWriter uses Harmony patches to modify vanilla RimWorld behavior. Harmony patches are located in the `HarmonyPatches` folder and typically follow the pattern:
csharp
using HarmonyLib;

public static class HarmonyPatch_Example
{
    [HarmonyPatch(typeof(ExampleClass), "ExampleMethod")]
    public static void Prefix(ref ExampleParameter param)
    {
        // Modify the parameter before the original method runs
        param = new ExampleParameter();
    }
}


## Suggestions for Copilot
1. **XML Definitions**: When generating XML files for new items, jobs, or research projects, ensure that the structure follows the existing conventions in the mod.
2. **Harmony Patches**: When creating Harmony patches, make sure to target the correct method and understand the implications of modifying the original code.
3. **C# Code**: When writing C# code for new features, ensure that it integrates well with the existing mod architecture and follows the coding conventions.

By following these guidelines, you can contribute effectively to the RimWriter mod and enhance the RimWorld experience for players.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

