# .github/copilot-instructions.md

## RimWorld Mod: RimWriter

### Mod Overview and Purpose

RimWriter is a mod for RimWorld designed to enhance the intellectual and cultural depth of the player's colony by introducing new interactions with books and writing tools. The mod provides colonists with the ability to read and write, offering new forms of entertainment and skill development. The primary goal is to enrich the storytelling aspect of the game by integrating written works as a central feature.

### Key Features and Systems

- **Building System**: 
  - **Building_Bookcase**: Functions as a storage unit for books, integrating with the internal storage system to manage book placements and categorization.
  - **Building_Typewriter**: Acts as a crafting station where colonists can create new written works.

- **Inventory Management**:
  - **ITab_Inventory**: Custom inventory tab allowing players to manage book storage and organization efficiently.

- **Book Interaction**:
  - **GuideBook & ThingBook**: Items that characters can engage with for learning and joy purposes.
  - **Item_JournalPage**: Collectible items that build up to create completed books or journals.

- **Job Systems**:
  - **JobDriver_FreeWrite & JobDriver_ReadABook**: Implement core activities related to reading and writing in the game.

- **Joy Activities**:
  - **JoyGiver_ReadABook**: Structure to facilitate book-related joy activities.

- **Room Roles**:
  - **RoomRoleWorker_Library**: Define new room roles for libraries, enhancing room classification logic.

### Coding Patterns and Conventions

- **Inheritance and Interfaces**: Usage of C# inheritance and interfaces such as `IThingHolder` and `IStoreSettingsParent` to manage complex object interactions like storage and item holding.
- **Private Methods**: Methods like `processInput` and `resolveOwner` in specific classes are marked private to encapsulate functionality and preserve clean class interfaces.
- **Public Methods**: Key functionalities are exposed through public methods to allow interaction with the mod's classes and systems easily.

### XML Integration

- The XML files support the mod by defining the data structures and properties for the custom items and buildings integrated into the game.
- It seems there is currently an error in parsing XML, which would need fixing to ensure mod objects are properly defined and utilized.

### Harmony Patching

- Harmony is used for modifying existing game behavior or injecting new logic where necessary. Recommend setting up standard Harmony patches to maintain game stability and ensure seamless integration with existing game mechanics.

### Suggestions for Copilot

- **Class Implementation**: When enhancing existing classes or creating new ones, ensure adherence to existing coding patterns and incorporate relevant interfaces.
- **Method Suggestions**: Utilize Copilot to generate helper methods for common tasks (e.g., complex inventory checks or addition of new job roles).
- **Error Handling**: Suggest improvements in error handling, especially concerning XML integration to prevent future parsing errors.
- **Performance Optimization**: Propose refactorings or optimizations that align with RimWorld's performance requirements, especially in high-load methods.
- **Documentation**: Regularly comment your code to assist Copilot in understanding the intended logic and providing more accurate suggestions.

This instruction file should provide a thorough grounding in the structure and functionality of the RimWriter mod, allowing for efficient use of GitHub Copilot to aid in its development.
