using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using System;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace KoreanPatchChecker
{
    // Main Mod class
    public class KoreanPatchChecker : Mod
    {
        internal static UserInterface CheckerInterface;
        internal static CheckerUI CheckerUI;
        internal static bool CheckerVisible = false;
        
        // Keybind for toggling the checker UI
        internal static ModKeybind ToggleUIKeybind;

        // List of mods to exclude from checking
        public static List<string> ExcludedMods = new List<string>
        {
            // Add more mods to exclude here as needed
        };
        
        // Helper method to add a mod to the exclude list
        public static void AddModToExcludeList(string modDisplayName)
        {
            if (!ExcludedMods.Contains(modDisplayName))
            {
                ExcludedMods.Add(modDisplayName);
            }
        }
        
        // Helper method to remove a mod from the exclude list
        public static void RemoveModFromExcludeList(string modDisplayName)
        {
            if (ExcludedMods.Contains(modDisplayName))
            {
                ExcludedMods.Remove(modDisplayName);
            }
        }

        // Mapping between original mods and their Korean patch mods
        // Using DisplayName as the key for better user understanding
        public static Dictionary<string, (string korPatchDisplayName, string workshopUrl)> KoreanPatchMap = new Dictionary<string, (string, string)>
        {
            // Example data: {"OriginalModDisplayName", ("KoreanPatchDisplayName", "SteamWorkshopURL")}
            {"tModLoader", ("[c/FF0000:1.4.4.9 ][c/FF8000:tMod][c/FFFF00:Loader ][c/008000:Kor][c/0000FF:ean][c/000080: Trans][c/800080:lation]", "https://steamcommunity.com/sharedfiles/filedetails/?id=2952627559")},
            {"AlchemistNPC Lite", ("AlchemistNPCLite_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3015285571")},
            {"Apotheosis & Friends", ("Apothesis and Friend KR 한글 패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3312879283&searchtext=KR")},
            {"ArmamentDisplay", ("ArmamentDisplay_kr", "https://steamcommunity.com/sharedfiles/filedetails/?id=3421952754&searchtext=KR")},
            {"Asphalt Platforms", ("AsphaltPlatforms_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3372926032&searchtext=KR")},
            {"Autofish", ("Autofish_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373147521&searchtext=KR")},
            {"Block's Info Accessories", ("Block's Info Accesories KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3323741486&searchtext=KR")},
            {"Block's Leveling Mod", ("Block's leveling mod 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3364883593&searchtext=KR")},
            {"Boss Checklist", ("BossChecklist_KR2", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373144799")},
            {"Calamity - Fargo's Souls DLC", ("fargo dlc_kr", "https://steamcommunity.com/sharedfiles/filedetails/?id=3416362237&searchtext=KR")},
            {"Calamity Entropy", ("Calamity Entropy KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3403087047&searchtext=KR")},
            {"Calamity Fables", ("Calamity Fables KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3449575527&searchtext=KR")},
            {"Calamity Magic Storage", ("CalamityMagicStorage KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3444714220&searchtext=KR")},
            {"Calamity Mod", ("Calamity Mod KR translation 칼라미티 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3014683484")},
            {"Calamity Mod Infernum Mode", ("Infernum Korean Localization", "https://steamcommunity.com/workshop/filedetails/?id=3015412343")},
            {"Calamity Ranger Expansion", ("Calamity Ranger Expansion_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3416822134&searchtext=KR")},
            {"Calamity's Vanities", ("Calamitys Vanities_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3418854322")},
            {"Calamity: Hunt of the Old God", ("Calamity Hunt of the Old God KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3308351474&searchtext=KR")},
            {"Calamity: Wrath of the Gods", ("Calamity: Wrath of the Gods Korean translation 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3215378717")},
            {"Catalyst Mod", ("Catalyst Mod KR Translation 카탈리스트 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3033308634")},
            {"Clamity Addon", ("Clamity Addon KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3443955743&searchtext=KR")},
            {"Consolaria", ("Consolaria KR Translation 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3017496221")},
            {"Corruption Core Boss", ("Corruption Core Boss KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3450530478&searchtext=KR")},
            {"DPSExtreme", ("DPSExtreme_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373624690&searchtext=KR")},
            {"DormantDawnMOD(沉睡黎明)", ("DormantDawnMOD KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3447184444&searchtext=KR")},
            {"DragonLens", ("Dragon Lens KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3325793876&searchtext=KR")},
            {"Ebonian Mod", ("Ebonian Mod KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3438421846&searchtext=KR")},
            {"Everjade", ("Everjade KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3300154117&searchtext=KR")},
            {"Fargo's Mutant Mod", ("Fargo's Mutant Mod KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3261641155")},
            {"Fargo's Souls Mod", ("Fargo's Souls Mod KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3261646997")},
            {"Fervent Arms", ("fervent arms korean localization 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3449433535&searchtext=KR")},
            {"Gensokyo", ("Gensokyo 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3038179850")},
            {"Homeward Journey", ("Homeward모드 한글", "https://steamcommunity.com/sharedfiles/filedetails/?id=3027702747")},
            {"Hook Stats and Wing Stats", ("HookStatsAndWingStats_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373149816&searchtext=KR")},
            {"Infected Qualities", ("Infected Qualities KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3450566031&searchtext=KR")},
            {"JoJoStands", ("Jojo Stands KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3386156936&searchtext=KR")},
            {"Jungle Bosses Rework", ("Jungle Bosses Rework KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3440419324&searchtext=KR")},
            {"Living World Mod [INDEV]", ("Living World Mod KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3439259270&searchtext=KR")},
            {"Lobotomy Corporation", ("Lobotomy Corparation KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3385551994")},
            {"Magic Storage", ("MagicStorage_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3018338773")},
            {"Mech Bosses Rework", ("Mech Bosses Rework mod korean localization 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3364345199&searchtext=KR")},
            {"More Lore - Calamity Lore for Non-Calamity Bosses!", ("More Lore KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3441439435&searchtext=KR")},
            {"More Trophies and Relics [Modded Support]", ("More Trophies and Relics KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3438886601&searchtext=KR")},
            {"Munchies - Calamity Addon", ("Munchies_CalamityAddon_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373827740&searchtext=KR")},
            {"Munchies - Consumables Checklist", ("Munchies_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3373115190&searchtext=KR")},
            {"Orchid Mineshaft", ("Orchid Mineshaft KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3451841233&searchtext=KR")},
            {"Orchid Mod", ("Orchid Mod KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3451791412&searchtext=KR")},
            {"Ore Excavator (1.4.3/1.4.4 Veinminer)", ("Ore Excavator_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3423255532")},
            {"Quality of Terraria", ("Quality of Life KR Translation 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3017400596")},
            {"Radiance", ("Radiance KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3447671155&searchtext=KR")},
            {"Ragnarok", ("Ragnarok Mod Korean Localization 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3365328100&searchtext=KR")},
            {"Recipe Browser", ("RecipeBrowser_KR2", "https://steamcommunity.com/sharedfiles/filedetails/?id=3380513934&searchtext=KR")},
            {"Remnants", ("Remnants_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3417727909")},
            {"Revengeance+", ("Revengeance+ KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3445615844&searchtext=KR")},
            {"Secrets Of The Shadows", ("Secret of The Shadows 한글", "https://steamcommunity.com/sharedfiles/filedetails/?id=3052976653")},
            {"Sloome (calamity Expansion)", ("Calamity Sloome KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3437264900&searchtext=KR")},
            {"Soul mod", ("Soul mod 한글", "https://steamcommunity.com/sharedfiles/filedetails/?id=3036391769")},
            {"Spirit Classic", ("Spirit Classic KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3430842886&searchtext=KR")},
            {"Spirit Reforged", ("Spirit Reforged KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3449429504&searchtext=KR")},
            {"Spooky Mod", ("Spooky mod KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3136377284")},
            {"Starlight River", ("Starlight_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3380496526&searchtext=KR")},
            {"Summoners' Association", ("SummonersAssociation_KR Korea 한글패치 / 한국어", "https://steamcommunity.com/sharedfiles/filedetails/?id=3027610031")},
            {"TerraTCG: PvP Update!", ("TCG_Kor", "https://steamcommunity.com/sharedfiles/filedetails/?id=3445788641&searchtext=KR")},
            {"Terramon Mod (Beta)", ("Terramon_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3445748357&searchtext=KR")},
            {"Terraria Overhaul (Configuration Update!)", ("TerrariaOverhaul_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3372911133&searchtext=KR")},
            {"Terraria: One Block Challenge", ("원블록 챌린지 한국어 패치(Terraria: One Block Challenge_kr)", "https://steamcommunity.com/sharedfiles/filedetails/?id=3448730380&searchtext=KR")},
            {"The Clicker Class", ("The Clicker Class_KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3359339728&searchtext=KR")},
            {"The Stars Above", ("The Stars Above KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3313777408")},
            {"The Story of Red Cloud", ("The Story of Red Cloud_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3298637733&searchtext=KR")},
            {"The Ultimate Infinite Star", ("The Ultimate Infinite Star Mod KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3322433683&searchtext=KR")},
            {"Thorium Bosses Reworked", ("Thorium Bosses Rework mod korean localization 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3365512467&searchtext=KR")},
            {"Touhou Little Friends ~ Adventure with cute partners", ("동방펫한글", "https://steamcommunity.com/sharedfiles/filedetails/?id=3217092610")},
            {"Twilight(薄暝)", ("Twilight KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3414798981&searchtext=KR")},
            {"Unofficial Calamity Bard & Healer", ("Unofficial Calamity Bard n Healer korean localization 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3364563601&searchtext=KR")},
            {"Unofficial Spirit Bard, Thrower & Healer", ("Unofficial Spirit Bard, Thrower&Healer KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3436290512&searchtext=KR")},
            {"Vacuum Ore Bag", ("VacuumOreBag_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3379722445&searchtext=KR")},
            {"WAYFAIR Content Pack [1.2: Secret Stones!]", ("WAYFAIR Content Pack KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3444863185&searchtext=KR")},
            {"Weapon Augments", ("Weapon Augments Korean Localization", "https://steamcommunity.com/sharedfiles/filedetails/?id=3362801786&searchtext=KR")},
            {"Weapon Enchantments", ("Weapon Enchantments Korean 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3332290796&searchtext=KR")},
            {"Wombat's General Improvements", ("WombatQOL_KR", "https://steamcommunity.com/sharedfiles/filedetails/?id=3417755832&searchtext=KR")},
            {"You", ("You KR 한글패치", "https://steamcommunity.com/sharedfiles/filedetails/?id=3438686034&searchtext=KR")},
            // Add more mod mappings here
        };
        
        // Common patterns for Korean patch mod names
        private static readonly List<string> _koreanPatchPatterns = new List<string>
        {
            "KR",
            "Korean",
            "한글",
            "한글패치",
            "한국어",
            "[c/008000:Kor][c/0000FF:ean]",
            "한패",
        };

        // Toggle checker UI method
        internal static void ToggleCheckerUI()
        {
            CheckerVisible = !CheckerVisible;

            if (CheckerVisible) {
                CheckerUI.UpdateModList();
                CheckerInterface.SetState(CheckerUI);
            } else {
                CheckerInterface.SetState(null);
            }
        }

        internal static List<(string displayName, string korPatchDisplayName, bool isInstalled, string workshopUrl, bool isAutoDetected)> CheckKoreanPatches()
        {
            var results = new List<(string displayName, string korPatchDisplayName, bool isInstalled, string workshopUrl, bool isAutoDetected)>();
            var allMods = ModLoader.Mods.Where(m => !ExcludedMods.Contains(m.DisplayName)).ToList();
            var koreanPatchCandidates = allMods.Where(m => IsKoreanPatchByName(m.DisplayName)).ToList();

            foreach (var mod in allMods)
            {
                if (koreanPatchCandidates.Contains(mod)) continue;

                bool isPatchInstalled = false;
                bool isAutoDetected = false;
                string korPatchDisplayName = "";
                string workshopUrl = $"https://steamcommunity.com/workshop/browse/?appid=1281930&searchtext={Uri.EscapeDataString(mod.DisplayName)}+한글";

                if (KoreanPatchMap.TryGetValue(mod.DisplayName, out var patchInfo))
                {
                    (korPatchDisplayName, workshopUrl) = patchInfo;
                    isPatchInstalled = allMods.Any(m => m.DisplayName == korPatchDisplayName);
                }
                else if (AutoDetectKoreanPatch(mod.DisplayName, koreanPatchCandidates) is (true, var detectedPatch))
                {
                    korPatchDisplayName = detectedPatch;
                    isPatchInstalled = true;
                    isAutoDetected = true;
                }

                results.Add((mod.DisplayName, korPatchDisplayName, isPatchInstalled, workshopUrl, isAutoDetected));
            }
            
            return results.Where(r => !ExcludedMods.Contains(r.displayName)).ToList();
        }
        
        // Method to check if a mod name suggests it's a Korean patch
        private static bool IsKoreanPatchByName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                return false;

            string lowerName = displayName.ToLowerInvariant();
            return _koreanPatchPatterns.Any(pattern => lowerName.Contains(pattern.ToLowerInvariant()));
        }

        private static (bool detected, string patchDisplayName) AutoDetectKoreanPatch(string originalDisplayName, List<Mod> koreanPatchCandidates)
        {
            string cleanOrigName = originalDisplayName.Replace(" ", "").ToLowerInvariant();

            foreach (var patchMod in koreanPatchCandidates)
            {
                string cleanPatchName = patchMod.DisplayName.Replace(" ", "").ToLowerInvariant();

                // 원본 이름이 한글 패치 이름에 포함되거나, KR/한글 패턴을 제외한 후 이름이 같으면 한글 패치로 감지
                if (cleanPatchName.Contains(cleanOrigName) ||
                    _koreanPatchPatterns.Any(pattern => cleanPatchName.Replace(pattern.ToLowerInvariant(), "") == cleanOrigName))
                {
                    return (true, patchMod.DisplayName);
                }
            }
            return (false, "");
        }
    }

    // ModSystem class to handle UI updates and key presses
    public class KoreanPatchCheckerSystem : ModSystem
    {
        public override void Load()
        {
            if (!Main.dedServ)
            {
                KoreanPatchChecker.CheckerUI = new CheckerUI();
                KoreanPatchChecker.CheckerUI.Initialize();
                KoreanPatchChecker.CheckerInterface = new UserInterface();
                KoreanPatchChecker.CheckerInterface.SetState(null);
                
                // Register keybind (default to K key)
                KoreanPatchChecker.ToggleUIKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle Korean Patch Checker", Keys.K);
                
                // Log excluded mods for debugging
                string excludedMods = string.Join(", ", KoreanPatchChecker.ExcludedMods);
                ModLoader.GetMod("KoreanPatchChecker").Logger.Info($"Korean Patch Checker: Excluded mods: {excludedMods}");
            }
        }

        public override void Unload()
        {
            KoreanPatchChecker.CheckerUI = null;
            KoreanPatchChecker.CheckerInterface = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (KoreanPatchChecker.CheckerVisible && KoreanPatchChecker.CheckerInterface?.CurrentState != null)
            {
                KoreanPatchChecker.CheckerInterface.Update(gameTime);
            }

            // Check for keybind press instead of direct key detection
            if (KoreanPatchChecker.ToggleUIKeybind.JustPressed)
            {
                KoreanPatchChecker.ToggleCheckerUI();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                    "KoreanPatchChecker: Interface",
                    delegate
                    {
                        if (KoreanPatchChecker.CheckerVisible && KoreanPatchChecker.CheckerInterface?.CurrentState != null)
                        {
                            KoreanPatchChecker.CheckerInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }

    // UI Class - Using only display names for UI elements
    class CheckerUI : UIState
    {
        internal UIPanel _mainPanel;
        private UIList _modList;
        private Asset<Texture2D> _workshopIcon;
        private KoreanPatchChecker _mod;

        public override void OnInitialize()
        {
            _mod = ModLoader.Mods.FirstOrDefault(m => m.Name == "KoreanPatchChecker") as KoreanPatchChecker;

            // Setup main panel
            _mainPanel = new UIPanel();
            _mainPanel.Width.Set(850f, 0f);
            _mainPanel.Height.Set(450f, 0f);
            _mainPanel.Left.Set(200f, 0f);
            _mainPanel.Top.Set(350f, 0f);
            
            _mainPanel.BackgroundColor = new Color(73, 94, 171);
            _mainPanel.BorderColor = new Color(89, 116, 213);
            
            // Header panel
            UIPanel titleBar = new UIPanel();
            titleBar.Width.Set(850f, 0f);
            titleBar.Height.Set(40f, 0f);
            titleBar.Left.Set(0f, 0f);
            titleBar.Top.Set(0f, 0f);
            titleBar.BackgroundColor = new Color(60, 80, 160);
            titleBar.BorderColor = new Color(89, 116, 213);
            
            // Title text
            UIText titleText = new UIText("Korean Patch Checker", 1.2f);
            titleText.Width.Set(0f, 1f);
            titleText.Height.Set(40f, 0f);
            titleText.Top.Set(0f, 0f);
            titleText.HAlign = 0.5f;
            titleText.VAlign = 0.5f;
            titleText.TextColor = Color.White;
            titleBar.Append(titleText);
            
            _mainPanel.Append(titleBar);
            
            // Mod list with scrolling capability
            UIPanel scrollPanel = new UIPanel();
            scrollPanel.Width.Set(850f, 0f);
            scrollPanel.Height.Set(380f, 0f);
            scrollPanel.Left.Set(0f, 0f);
            scrollPanel.Top.Set(50f, 0f);
            scrollPanel.BackgroundColor = new Color(25, 25, 75);
            scrollPanel.BorderColor = new Color(35, 35, 85);
            _mainPanel.Append(scrollPanel);
            
            // Create scroll bar
            UIScrollbar scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(375f, 0f);
            scrollbar.Left.Set(790f, 0f);
            scrollbar.Top.Set(0f, 0f);
            scrollPanel.Append(scrollbar);
            
            // Mod list
            _modList = new UIList();
            _modList.Width.Set(785f, 0f);
            _modList.Height.Set(375f, 0f);
            _modList.Left.Set(0f, 0f);
            _modList.Top.Set(0f, 0f);
            _modList.ListPadding = 8f; // Increased padding
            
            // Set the scrollbar to control this list
            _modList.SetScrollbar(scrollbar);
            scrollPanel.Append(_modList);
            
            Append(_mainPanel);
        }

        public void UpdateModList()
        {
            _modList.Clear();
            var patches = KoreanPatchChecker.CheckKoreanPatches()
                .Where(patch => !KoreanPatchChecker.ExcludedMods.Contains(patch.displayName)) // 필터링 한 번에 처리
                .OrderByDescending(patch => patch.isInstalled) // 설치된 한글 패치를 위로
                .ThenBy(patch => patch.displayName) // 이름순 정렬
                .ToList();

            foreach (var patch in patches)
            {
                var itemPanel = new UIPanel { Width = new StyleDimension(800f, 0f), Height = new StyleDimension(30f, 0f) };
                itemPanel.BackgroundColor = new Color(50, 50, 120);

                var modNameText = new UIText(patch.displayName, 1f) { Left = new StyleDimension(15f, 0f), VAlign = 0.5f };
                itemPanel.Append(modNameText);

                var statusText = new UIText(patch.isInstalled ? $"{patch.korPatchDisplayName}" : "Click to Subscribe Korean Patch", 1f)
                {
                    Left = new StyleDimension(350f, 0f),
                    VAlign = 0.5f,
                    TextColor = patch.isInstalled ? Color.Green : Color.Red
                };

                if (!patch.isInstalled)
                {
                    statusText.OnLeftClick += (evt, element) => OpenUrl(patch.workshopUrl);
                    statusText.OnMouseOver += (evt, element) => statusText.TextColor = Color.Yellow;
                    statusText.OnMouseOut += (evt, element) => statusText.TextColor = Color.Red;
                }

                itemPanel.Append(statusText);
                _modList.Add(itemPanel);
            }
        }
        
        private void OpenUrl(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
                else
                {
                    Main.NewText("Unsupported OS for opening URL.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                Main.NewText("Failed to open URL: " + ex.Message, Color.Red);
            }
        }
    }
}