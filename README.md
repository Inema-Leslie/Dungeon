Chronicles of a Lost Dungeon/ Prison Break
This is a 3D third-person game built in Unity6. The player fights through 5 interconnected levels to escape a prison, battling 4 distinct enemies.

Gameplay
Level	Objective
1	Break out of your chains and find a weapon
2	Defeat the Black Paladin (Warrior)
3	Save the child from the Monster
4	Get past the Archers
5	Defeat the Guardian and escape the prison
Enemy types
1. Warrior hit-count-based duel; defeating him completes Level 2 and unlocks Level 3
2. Monster hostage/child-rescue encounter with hit-counter-based combat and its own death sequence
3. Archers  ranged enemies using Object Pooling for arrows and a line-of-sight raycast so they stop shooting through walls.
4. Guardian — the final boss guarding the exit; defeating him triggers the Victory sequence and marks the game as beaten on the player's profile.

Player Abilities
Implemented via the Strategy pattern (IAbility):
Dash (Left Shift / Gamepad Right Shoulder)
Heal (H / Gamepad Left Shoulder) — consumes a Heal Potion charge picked up separately in the world
Controls
Action	Keyboard/Mouse	Gamepad	Touch (Mobile)
Move	WASD / Arrow Keys	Left Stick	On-screen stick
Look	Mouse	Right Stick	Touch drag
Attack	Left Click / Enter	Button West	On-screen button
Interact	E	Button North	On-screen button
Dash	Left Shift	Right Shoulder	On-screen button
Heal	H	Left Shoulder	On-screen button
Jump	Space	Button South	—
Crouch	C	Button East	—
Sprint	Left Shift	Left Stick Press	—
Architecture & Design Patterns

The project applies five design patterns throughout:

Singleton: GameManager, SaveManager, PlayerInventory, AudioManager, and PlayerProfileManager are all persistent (DontDestroyOnLoad) singletons instantiated once from the MainMenu scene, ensuring a single source of truth for global game state across all five levels (which share a single continuous scene).
Observer: GameEvents.cs centralizes all cross-system communication via C# events (e.g. OnPlayerDied, OnGuardianDefeated, OnHealthChanged, OnItemCollected). Systems like UI, audio, and save data subscribe independently without any direct references to each other, keeping Health, combat, and enemy scripts fully decoupled from the systems that react to them.
State: Each enemy with complex behavior (WarriorBehaviour, MonsterBehaviour, Archer and Guardian equivalents) implements its own state machine (Idle, Chase, Attack, Dead states) so behavior transitions are self-contained and easy to extend.
Strategy: Player abilities (IAbility: Dash, Heal) are interchangeable objects invoked through a common interface, allowing new abilities to be added without modifying input-handling code.
Dependency Inversion via Interfaces: Combat, damage, and enemy behavior all depend on interfaces (IDamageable, IEnemyBehaviour, IAttackable) rather than concrete classes, so new enemies or attackers can be added without modifying existing systems (see Interfaces).
Interfaces

Five interfaces drive the project's extensibility:

IDamageable: implemented by every enemy's Health component and the player; anything that can take damage and die exposes TakeDamage() and Die(). PlayerCombat resolves hit targets purely through this interface (GetComponentInParent<IDamageable>()), meaning it has zero awareness of which specific enemy type it just hit.
IEnemyBehaviour: implemented by MonsterBehaviour, WarriorBehaviour, and other enemy scripts; standardizes UpdateBehaviour() and OnPlayerDetected() so any AI system driving enemies can treat them uniformly.
IAttackable: implemented by enemies capable of dealing damage back to the player (e.g. WarriorBehaviour.Attack()).
IAbility: implemented by DashAbility and HealAbility; each exposes an Execute()-style entry point so the input system can trigger any ability without knowing its internal logic.

Algorithms
Gameplay Logic: Health Regeneration (Health.cs): time-managed interpolation that begins regenerating HP after 4 seconds without damage, at a fixed rate per second, until reaching max health.
Navigation: Archer Line-of-Sight (Archer behaviour scripts): a raycast between archer and player each frame determines whether geometry obstructs the shot; archers hold fire if blocked, making cover a meaningful tactical option.
Sorting: Bubble Sort for Inventory Display (InventorySorter.cs): sorts collected items alphabetically for the inventory panel. O(n²) complexity is an acceptable and deliberate tradeoff given the small, bounded size of the player's inventory — simplicity and readability outweigh asymptotic performance at this scale.

REST API Integration
PlayerProfileManager.cs integrates with JSONBin.io to persist a player profile server-side:
GET retrieves the existing profile on load.
PUT saves the player's submitted name and hasBeatenGame status.
The Main Menu's name input field and submit button call MainMenuManager.OnNameSubmitted() → PlayerProfileManager.SetPlayerName().
GuardianBehaviour.OnDefeated() calls PlayerProfileManager.Instance?.MarkGameBeaten() once the Guardian is defeated, updating the player's profile to reflect game completion.

Unit Tests
10 unit tests across 6 test files :
GameManagerTests
HealthTests
PlayerInventoryTests
SaveManagerTests
GuardianStateTests
ArrowPoolTests
Multi-Platform Support

The game builds and runs on three platforms
Windows (PC Standalone) — baseline build, full file I/O save support.
WebGL — save system adapted via conditional compilation (see above) to persist correctly in the browser sandbox.
Mobile (Android) — on-screen touch controls (movement stick + Attack/Interact/Dash/Heal buttons) built using Unity's Input System On-Screen Controls, mapped onto existing Gamepad bindings in InputSystem_Actions so no changes were needed to PlayerMovement, PlayerCombat, or ability scripts. MobileUIController.cs shows/hides these controls based on platform via #if UNITY_ANDROID || UNITY_IOS.
Input differences, UI scaling (Canvas Scaler set to Scale With Screen Size), and platform-specific save behavior were all considered in the above adaptations.
Setup & Build Instructions
Clone the repository and open in Unity 6000.4.5f1 (or later).
Ensure the Android Build Support module (with SDK & NDK Tools) is installed via Unity Hub if building for Android.
Always launch from the MainMenu scene, not SampleScene directly — persistent singletons (GameManager, SaveManager, PlayerInventory, AudioManager, PlayerProfileManager) are only instantiated from MainMenu and testing from SampleScene directly will cause null-reference errors.
Windows Build: File → Build Profiles → Windows → Build.
WebGL Build: File → Build Profiles → Web → Build. Test locally via a local server (python -m http.server 8000), not by opening index.html directly, due to browser CORS restrictions.
Android Build: File → Build Profiles → Android → Build. Requires a compatible NDK version as specified by your Unity Editor version.
