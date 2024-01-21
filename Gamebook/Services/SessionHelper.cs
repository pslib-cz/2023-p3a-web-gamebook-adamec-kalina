using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Global = Gamebook.GlobalModels.GlobalModels;

namespace Gamebook.Services;
// SecretMeetingPlace, Workshop lock
public class SessionHelper : ISessionHelper
{
    private readonly IHttpContextAccessor _httpContext;
    
    //Default game locations' states
    private Dictionary<Location, GameLocation> gameLocationDataDict = new()
    {
        {Location.SlumDistrict, new GameLocation(){Title = "Slum District", BackgroundImage = "slum-district", Locked = false}},
        {Location.SlumQuarter, new GameLocation(){Title = "Slum Quarter", BackgroundImage = "slum-quarter", Locked = false, }},
        {Location.ElectroShop, new GameLocation(){Title = "Electro Shop", BackgroundImage = "electro-shop"}},
        {Location.DarkAlley, new GameLocation(){Title = "Dark Alley", BackgroundImage = "dark-alley", Hitboxes = new List<Hitbox>(){new(){Type = HitboxType.Fight, PlayerDealingType = PlayerDealingType.Violent, HitboxOrder = new(){Quest = 1,Step = 4}}}}},
        {Location.ShadyBar, new GameLocation(){Title = "Shady Bar", BackgroundImage = "shady-bar"}},
        {Location.PartOfTheBar, new GameLocation(){Title = "Part of the Bar", BackgroundImage = "part-of-the-bar"}},
        {Location.BackEntrance, new GameLocation(){Title = "Back Entrance", BackgroundImage = "back-entrance", Hitboxes = new List<Hitbox>(){new(){Type = HitboxType.Pin, HitboxOrder = null}}}},
        {Location.SecretMeetingPlace, new GameLocation(){Title = "Secret Meeting Place", BackgroundImage = "secret-meeting-place",Locked = false, Hitboxes = new List<Hitbox>(){new(){Type = HitboxType.Hack, HitboxOrder = new(){Quest = 2, Step = 4}}}}},
        {Location.Workshop, new GameLocation(){Title = "Workshop", BackgroundImage = "workshop"}},
        {Location.TacticalRoom, new GameLocation(){Title = "Tactical Room", BackgroundImage = "tactical-room", Locked = false}},
        {Location.CyberLab, new GameLocation(){Title = "Cyber Lab", BackgroundImage = "cyber-lab"}},
        {Location.QuantumTechnology, new GameLocation(){Title = "Quantum Technology", BackgroundImage = "quantum-technology"}},
                
        {Location.CorporalBuilding, new GameLocation(){Title = "Corporal Building", BackgroundImage = "corporal-building" }},
        {Location.MainEntrance, new GameLocation(){Title = "Main Entrance", BackgroundImage = "main-entrance", Locked = false}},
        {Location.Reception, new GameLocation(){Title = "Reception", BackgroundImage = "reception", Locked = false}},
        {Location.BackOfBuilding, new GameLocation(){Title = "Back Of Building", BackgroundImage = "back-of-building", Locked = false}},
        {Location.Hallway, new GameLocation(){Title = "Hallway", BackgroundImage = "hallway", Locked = false}},
        {Location.ServerRoom, new GameLocation(){Title = "Server Room", BackgroundImage = "server-room", Locked = false}},
        {Location.SecurityDoor1, new GameLocation(){Title = "Security Door", BackgroundImage = "security-door", Locked = false}},  // needs keycard
        {Location.SecurityDoor2, new GameLocation(){Title = "Security Door", BackgroundImage = "security-door", Locked = false}},  // needs keycard
        {Location.HoldingCells, new GameLocation(){Title = "Holding Cells", BackgroundImage = "holding-cells", Locked = false}}, //Locked = true
        {Location.ControlRoom, new GameLocation(){Title = "Control Room", BackgroundImage = "control-room", Locked = false}},
        {Location.ChiefTechnitianOffice, new GameLocation(){Title = "Chiefs Technician Office", BackgroundImage = "chief-technician-office", Locked = false}},
        {Location.BackDoor, new GameLocation(){Title = "Back Door", BackgroundImage = "back-door", Locked = false}},
        {Location.Warehouse, new GameLocation(){Title = "Warehouse", BackgroundImage = "warehouse", Locked = false}},
        {Location.BoilerRoom, new GameLocation(){Title = "Boiler Room", BackgroundImage = "boiler-room", Locked = false}},
        {Location.SecurityRoom, new GameLocation(){Title = "Security Room", BackgroundImage = "security-room", Locked = false}},
        {Location.ExtractionVan, new GameLocation(){Title = "Extraction Van", BackgroundImage = "extraction-van", Locked = false}},
                        
        {Location.Facility, new GameLocation(){Title = "Facility", BackgroundImage = "facility"}},
        {Location.DeliveryEntrance, new GameLocation(){Title = "Delivery Entrance", BackgroundImage = "delivery-entrance", Locked = false}},
        {Location.SecurityCheckpoint, new GameLocation(){Title = "Security Checkpoint", BackgroundImage = "security-checkpoint", Locked = false}},
        {Location.ElectricalHallway, new GameLocation(){Title = "Electrical Hallway", BackgroundImage = "electrical-hallway", Locked = false}},
        {Location.RoomA421, new GameLocation(){Title = "Room A421", BackgroundImage = "room-A421", Locked = false}},
        {Location.RoomB78, new GameLocation(){Title = "Room B78", BackgroundImage = "room-B78", Locked = false}},
        {Location.RoomA765, new GameLocation(){Title = "Room A765", BackgroundImage = "room-A765", Locked = false}},
        {Location.MaintenanceArea, new GameLocation(){Title = "Maintenance Area", BackgroundImage = "maintenance-area", Locked = false}},
        {Location.ToolStorage, new GameLocation(){Title = "Tool Storage", BackgroundImage = "tool-storage", Locked = false}},
        {Location.EmergencyExit, new GameLocation(){Title = "Emergency Exit", BackgroundImage = "emergency-exit", Locked = false}}, // needs 3 cables
        {Location.EscapeCar, new GameLocation(){Title = "Escape Car", BackgroundImage = "escape-car", Locked = false}}, //Locked = true

        {Location.DataDepot, new GameLocation(){Title = "Data Depot", BackgroundImage = "data-depot"}},
        {Location.ThirdFloor, new GameLocation(){Title = "Third Floor", BackgroundImage = "third-floor", Locked = false}},
        {Location.MainLobby, new GameLocation(){Title = "Main Lobby", BackgroundImage = "main-lobby", Locked = false}},
        {Location.SectorD4, new GameLocation(){Title = "Sector D4", BackgroundImage = "sector-D4", Locked = false}},
        {Location.Server17, new GameLocation(){Title = "Server 17", BackgroundImage = "server-17", Locked = false}},
        {Location.Server21, new GameLocation(){Title = "Server 21", BackgroundImage = "server-21", Locked = false}},
        {Location.ResearchWing, new GameLocation(){Title = "Research Wing", BackgroundImage = "research-wing", Locked = false}},
        {Location.ComputerA765, new GameLocation(){Title = "Computer A765", BackgroundImage = "computer-A765", Locked = false}},
        {Location.RooftopExit, new GameLocation(){Title = "Rooftop Exit", BackgroundImage = "rooftop-exit", Locked = false}}, // needs 2x cables 1x chip
        {Location.Helipad, new GameLocation(){Title = "Helipad", BackgroundImage = "helipad", Locked = false}}, //Locked = true

        {Location.CorporateHeadquarters, new GameLocation(){Title = "Corporate Headquarters", BackgroundImage = "corporate-headquarters"}},
        {Location.Elevator, new GameLocation(){Title = "Elevator", BackgroundImage = "elevator", Locked = false}},
        {Location.Penthouse, new GameLocation(){Title = "Penthouse", BackgroundImage = "penthouse", Locked = false}},
        {Location.Boss, new GameLocation(){Title = "Boss", BackgroundImage = "boss", Locked = false}},


    };

    // Default game locations dialog states
    private Dictionary<string, List<Dialog>> gameLocationDialogDict = new()
    {
        {$"{Location.SlumQuarter}Dialog", new List<Dialog>()
            { new() {Unlock = new() { Location.ElectroShop}, DialogOrder = new(){Quest = 1, Step = 1}, Texts = new List<string> {  
                "Shadow Viper: Hello, you seem to have been here a long time. I don't remember my past. Do you know anything about me?",
                "Old Merchant: Hmm, your face is familiar, but there are a lot of people hanging around this neighborhood. I might remember something, but I have my own worries right now.",
                "Shadow Viper: Can I help you with something?",
                "Old Merchant: Well, I need a component to repair a piece of equipment. You can find it at Jake's electronics shop just down the road. Bring it to me and maybe I'll remember something useful."
            }}, new() {ItemsRemove = new() { Item.Battery}, DialogOrder = new(){Quest = 1, Step = 6, LastStep = true}, Unlock = new() { Location.ShadyBar}, Texts = new List<string>{
                "Shadow Viper: I have the component you needed.",
                "Old Merchant: Oh, that's it. Thanks, young man. Well, I promised you the information. I saw you come into the Quarter. You were accompanied by a strange figure, looked like a hacker or something. You had some electronics on you, and it looked like you were planning something together.", 
                "Shadow Viper: Do you know anything more about the character or what we were planning?",
                "Old Merchant: Unfortunately no, that's all I saw. But the character was wearing a badge I've seen on people in a group that meets in a shady bar on the outskirts of the neighborhood. Maybe you'll find more answers there."
                
            }}
        }},
        {$"{Location.ElectroShop}Dialog", new List<Dialog>()
            { new() {Unlock = new() { Location.DarkAlley }, DialogOrder = new(){Quest = 1, Step = 2}, Texts = new List<string> {
                "Shadow Viper: Hello, the Old Merchant sent me to you. He needs a component for repair.",
                "Jake: Oh, the old jerk. Oh well, I know what he needs. But it's not free. You have to promise me something.",
                "Shadow Viper: What do you want in return?", "Jake: I've been having trouble with a local gang lately. I need someone to make sure they leave me alone. You don't have to confront them directly, just let them know it would be better to leave me alone.",
                "Shadow Viper: What am I supposed to do? I have no power or influence here.",
                "Jake: You're big and you look pretty dangerous. All you have to do is show up in their territory and show off a little. Talk about joining a bigger group, come up with something. They're pretty paranoid, so that's enough.", 
                "Shadow Viper: All right, I'll do it. Then you'll give me the component?",
                "Jake: Yes, you have my word. When you do, the component is yours."
            }}, new() {ItemsAdd = new() { Item.Battery}, ItemsRemove = new() { Item.Eye}, DialogOrder = new(){Quest = 1, Step = 5}, Texts = new List<string>()
            {
                "Shadow Viper: I've dealt with the gangs. They should leave you alone.",
                "Jake: Good job, now I can hopefully concentrate on my work. Here's the component for the Old Merchant. Thanks for your help."
            }}
        }},
        {$"{Location.DarkAlley}Dialog", new List<Dialog>()
            { new() {DialogOrder = new(){Quest = 1, Step = 3}, SpecialType = PlayerDealingType.Violent, Texts = new List<string> {
                "Shadow Viper: I hear you're making trouble for Jake. I don't like that.", 
                "Gang Leader: What are you going to do to us? We have our territory here.",
                "Shadow Viper: I don't have to put it into words. I can take you all out without breaking a sweat."
            }}, new() {ItemsAdd = new() { Item.Eye}, DialogOrder = new(){Quest = 1, Step = 4}, SpecialType = PlayerDealingType.Violent, Texts = new List<string>()
            {
                "Gang Leader: 'Okay, okay, we'll leave him alone. Just go away and leave us alone."
            }}, new() {ItemsAdd = new() { Item.Eye}, DialogOrder = new(){Quest = 1, Step = 3}, SpecialType = PlayerDealingType.Peaceful, Texts = new List<string>()
            {
                "Shadow Viper: Rumor has it you have a problem with Jake, the electronics dealer.",
                "Gang Leader: Who wants to know?",
                "Shadow Viper: I'm a middleman. Cuba has connections to some very powerful people. You don't want to mess with him.",
                "Gang Leader: And why should we trust you?",
                "Shadow Viper: You don't have to. But if you have problems with Cuba, you can be sure that people who are more dangerous than me will come after you.",
                "Gang Leader: All right, we'll leave Cuba alone. But remember, we're watching you."
            }}
        }},
        {$"{Location.ShadyBar}Dialog", new List<Dialog>()
            {new() {Unlock = new() { Location.PartOfTheBar, Location.BackEntrance}, DialogOrder = new(){Quest = 2, Step = 1}, Texts = new List<string> { 
                "Shadow Viper: Hello, I hear this is a place where interesting people meet.",
                "Bartender: Interesting, you say? That depends on what you consider interesting.",
                "Shadow Viper: I'm looking for a group of people who aren't too keen on the way corporations run the city. I've heard that they might congregate here.",
                "Bartender: Many people have a problem with corporations, but few have the courage to do anything about it. Why do you ask?",
                "Shadow Viper: I have reason to believe I'm part of this group... but I have gaps in my memory. I need to get in touch with them.",
                "Bartender: So you have gaps in your memory, huh? That's pretty common around here. But if you're really one of them, you should know how to connect with them.",
                "Shadow Viper: I know they have meetings here. Could you tell me where I can find them?",
                "Bartender: I'm not saying I know anything, but people like you sometimes use that back entrance. Maybe you should try there."
            }}
        }},
        {$"{Location.PartOfTheBar}Dialog", new List<Dialog>()
            { new() { DialogOrder = new(){Quest = 2, Step = 2}, Texts = new List<string> {
                "Shadow Viper: Good evening, I heard you might know something about a group that meets in secret.",
                "Mysterious Guest:  Maybe yes, maybe no. Why would I tell a stranger anything?",
                "Shadow Viper: I need to meet them. I have important information they might be interested in.",
                "Mysterious Guest: Everyone has 'important information'. But okay, maybe I can help you. The back door uses a combination lock. The code is a four digit number. If you're smart enough, you should be able to guess it.",
                "Shadow Viper: What's the code?",
                "Mysterious Guest: I won't tell you the code directly, but I'll give you a hint. ",
                "Mysterious Guest: Last is 1 above the first, second to last is 1 above the second, no numbers are repeating."
            }}
        }},
        {$"{Location.SecretMeetingPlace}Dialog", new List<Dialog>()
            { new() { DialogOrder = new(){Quest = 2, Step = 3}, Texts = new List<string> {
                "Shadow Viper: I was told I would find you here. I'm Shadow Viper, and I'm looking for answers about my past.",
                "Hacktivist Leader: Shadow Viper, I've heard of you. There is talk that you have memory problems, but that you were once one of us. Why should we believe you?",
                "Shadow Viper: I've lost my memories, but I feel I have reasons to fight the corporations. I can help you.",
                "Hacktivist Leader: Many people want to fight the corporations, but few have the actual ability or courage to do so. How can you help?",
                "Shadow Viper: I have cybernetic abilities and I'm willing to use them against corporations. I just need a chance to prove it.",
                "Hacktivist Leader: Cyber abilities, you say? Okay, let's see what you can do. We have an assignment here that could test your abilities.",
                "Shadow Viper: I'm ready. What do you need me to do?",
                "Hacktivist Leader: We need to retrieve information from a corporate database. It's well protected, but with your abilities, you could do it. If you can do that, you'll prove to us that you're on our side.",
                "Shadow Viper: Understood. I'll give you the information.",
                "Hacktivist Leader: Alright, Shadow Viper. This quest will show you if you have what it takes to be part of our group. Don't let us down."
            }}, new() {DialogOrder = new(){Quest = 2, Step = 4, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: Mission accomplished. The information from the corporate database is yours.",
                "Hacktivist leader: Impressive. Not only did you do it, but you did it with such efficiency and discretion. Looks like your abilities aren't just empty words.", 
                "Shadow Viper: I want to learn more about my past and fight those who control this city. I believe your group may be the key.",
                "Hacktivist Leader: Your past remains a mystery, but your current abilities and determination speak for you. You are welcome in our group, Shadow Viper.",
                "Shadow Viper: Thank you. What's the next step?",
                "Hacktivist Leader: Now that you are part of our group, you will be involved in important missions. We have a plan to weaken the corporations and expose their dark secrets.",
                "Hacktivist leader: Get ready, Shadow Viper. The fight you've just joined won't be easy. But with your skills and our strategy, we have a chance to change things for the better."
            }}, new(){Unlock = new() { Location.CyberLab}, DialogOrder = new(){Quest = 3, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist Leader: Shadow Viper, your last mission showed that you are a valuable member of our team. But to face greater challenges, you need better equipment.",
                "Shadow Viper: I agree. I feel I can do more, but my current abilities have their limits.", 
                "Hacktivist Leader: That's why we'll put you in touch with our Ripper Dock. He'll help you select and install cybernetic enhancements.",
                "Shadow Viper: Sounds good. What kinds of upgrades are possible?",
                "Hacktivist Leader: From increasing your physical strength to expanding your hacking abilities. The choice is yours, but remember, every decision has consequences.",
                "Shadow Viper: Understood. I am prepared to choose the best to accomplish our goals.",
                "Hacktivist Leader: All right, Shadow Viper. An expert is waiting for you in our workshop. Good luck."
            }}, new() {DialogFocus = PlayerFocus.Physics, Unlock = new() { Location.TacticalRoom }, DialogOrder = new(){Quest = 4, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist leader: 'Shadow Viper, we have a crisis situation. One of our key members has been captured by the corporation. We have reason to believe he is being held in one of their security facilities.",
                "Shadow Viper: Capture? That sounds like a serious problem. What exactly happened?", 
                "Hacktivist Leader: He was captured during reconnaissance. We believe he has valuable information they could use against us.",
                "Shadow Viper: Understood. And what do you expect me to do?", 
                "Hacktivist Leader: We need you to use your new physical abilities to infiltrate their facility and save them before it's too late. Your abilities make you an ideal candidate for this mission.",
                "Shadow Viper: I accept the mission. Do you know where they're holding him?",
                "Tech Expert: Yes, we have the coordinates. However, the building is well guarded. You'll need a plan.", 
                "Shadow Viper: I'll get ready. No member of our group can be left in the hands of the corporation.",
                "Hacktivist Leader: Okay, Shadow Viper. Counting on you. Be careful, but be quick."
            }}, new() {DialogFocus = PlayerFocus.Physics, DialogOrder = new(){Quest = 4, Step = 3, LastStep = true}, Texts = new List<string>()
            {
                "Hacktivist Leader: Well done, Shadow Viper. Your actions tonight saved one life and provided us with valuable information.",
                "Shadow Viper: I'm just glad I could help. It's time to show the corporations that we're not afraid to fight back.",
                "Hacktivist Leader: Your success tonight has strengthened our position. We'll be counting on you for future missions."
            }}, new(){DialogFocus = PlayerFocus.Hack, Unlock = new() { Location.Workshop }, DialogOrder = new(){Quest = 4, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist leader: We have a key task for you. We need to hack into a corporate database and obtain information that can greatly aid our cause.",
                "Shadow Viper: What information are we looking for?",
                "Hacktivist Leader: We are looking for detailed plans and communications regarding new corporate projects that could have a devastating impact on the public. This data is stored in a well-protected database.",
                "Shadow Viper: I see. What are the security protocols for this database?", 
                "Hacktivist Leader: It's complicated. The database is protected by a series of firewalls, encryption protocols, and anti-hacking systems. But with your new skills, you should be able to break through them.",
                "Shadow Viper: Sounds like a challenge. What approach are we going to use?", 
                "Hacktivist Leader: Best bet is to use a direct neural link to our servers. I'll give you access to the latest hacking tools and real-time assistance.", 
                "Shadow Viper: Okay, I'm ready to dive into the system and get the information.",
                "Hacktivist Leader: Remember, Shadow Viper, time is of the essence. The longer you are inside the system, the greater the risk of detection. Be quick and careful.", 
                "Shadow Viper: I understand the risks. We'll start now."
            }}, new(){DialogFocus = PlayerFocus.Hack, DialogOrder = new(){Quest = 4, Step = 3, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: Mission accomplished. Information is secure and safe.",
                "Hacktivist leader: This is a huge success. This information will give us deep insight into corporate plans. Well done, Shadow Viper.",
                "Hacktivist Leader: Your success tonight has taken our cause a big step forward. Your skills as a hacker are invaluable to us.",
                "Shadow Viper: I'm glad to be able to contribute. What's next?",
                "Hacktivist Leader: Now we must analyze the data and prepare our next steps. Your role in our future missions will be crucial.",
                "Hacktivist leader: And make sure you take a moment to rest. These neural connections can be taxing on the brain.",
                "Shadow Viper: I'll take a break, but not for long. There's still a lot of work to do.",
            }}, new(){DialogFocus = PlayerFocus.Physics, Unlock = new() { Location.TacticalRoom }, DialogOrder = new(){Quest = 5, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist leader: We have an urgent situation. A corporation is about to launch a new project that could have serious consequences for the public. We need to stop it before it's too late.",
                "Shadow Viper: What exactly should I do?",
                "Hacktivist Leader: We need you to infiltrate one of their key industrial facilities and sabotage this project to stop it.",
                "Shadow Viper: What security does that facility have?",
                "Hacktivist Leader: It's well guarded, with state of the art security. But with your physical abilities, you should be able to get in and perform the necessary actions.", 
                "Shadow Viper: Sounds like a challenge. What's the plan?",
                "Hacktivist Leader: You need to get in, find key systems, and cause damage that will slow or completely shut down their operations. It's important to make it look like an accident to avoid direct communication with our group.",
                "Shadow Viper: Understood. I'll need detailed information on the layout of the facility and its systems.",
                "Hacktivist Leader: We'll provide everything you need. It's a risky operation, but we're confident you can handle it.",
                "Shadow Viper: All right, I'll get ready and then I'll be on my way."
            }}, new(){DialogFocus = PlayerFocus.Physics, DialogOrder = new(){Quest = 5, Step = 3, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: The sabotage was successful. Systems are down, and that should delay the corporation for quite some time.",
                "Hacktivist Leader: Well done, Shadow Viper. Your actions have caused considerable complications for the corporation and given us valuable time to plan further.",
                "Hacktivist Leader: What was it like in there? Did you run into any problems?",
                "Shadow Viper: There were a few guards, but I managed to get past them. My new abilities proved crucial to the success of the mission.",
                "Hacktivist Leader: Your skills and courage are invaluable to us. This sabotage gives us more than just time. It strengthens our position and gives us more influence.",
                "Shadow Viper: I'm ready for the next mission. Whatever you need, I'm here to help."
            }}, new(){DialogFocus = PlayerFocus.Hack, DialogOrder = new(){Quest = 5, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist Leader: We have a problem, Shadow Viper. Our networks are facing coordinated cyber attacks. It's the most aggressive we've ever seen.",
                "Shadow Viper: Who is behind these attacks?",
                "Hacktivist Leader: All indications are that it's corporate hackers. They appear to be targeting our operations and infrastructure directly.",
                "Shadow Viper: How serious are these attacks? Is our data safe?",
                "Hacktivist Leader: So far we have kept most of our data safe, but if we don't stop these attacks, we risk losing key information and compromising our operations.",
                "Shadow Viper: I see. What's the defense plan?",
                "Hacktivist Leader: We need you to use your hacking skills to identify the source of these attacks and stop them. You may need to counterattack to protect our systems.",
                "Shadow Viper: I'll prepare to invade their systems and find out what they're up to. I'll need access to our best tools and support.",
                "Hacktivist Leader: Everything you need is ready. You need to act fast, we don't have much time.",
                "Shadow Viper: We'll start now. This information war must not be lost."
            }}, new(){DialogFocus = PlayerFocus.Hack, DialogOrder = new(){Quest = 5, Step = 2, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: The attacks have been stopped. The source has been identified and neutralized. Our networks are now more secure.",
                "Hacktivist Leader: That's excellent work, Shadow Viper. Your cyber defense skills just saved our skins. What was the source of the attacks?",
                "Shadow Viper: I was able to trace the attackers back to one of the corporations. It seems they were trying to gain information on our activities and possibly sabotage our operations.",
                "Hacktivist Leader: This is important information. We need to be vigilant and prepared for other possible attacks. Your work tonight has provided us with valuable time and space.",
                "Shadow Viper: I am prepared to face any further challenges. I will make sure our networks remain secure."
            }}, new(){DialogOrder = new(){Quest = 6, Step = 1}, Texts = new List<string>()
            {
                "Hacktivist Leader: Shadow Viper, we have a crucial task ahead of us. The Corporation is developing a new security system that could seriously compromise our ability to operate.",
                "Shadow Viper: Sounds serious. What exactly am I supposed to do?",
                "Hacktivist Leader: This system is controlled from a highly secure facility. Your mission is to infiltrate this facility, access its central computer, and use your hacking skills to take control of the system.",
                "Shadow Viper: That means a combination of physical penetration and digital manipulation. A challenge I am prepared to accept.",
                "Hacktivist Leader: We know you can do it. Your skills in both areas are now essential to us.",
                "Hacktivist Leader: We will provide you with all the information you need on device security measures and the latest versions of hacking tools.",
                "Shadow Viper: What's the situation inside?",
                "Hacktivist Leader: The facility is guarded by physical security and has advanced digital security. You'll have to be careful to avoid detection.",
                "Shadow Viper: Okay, we'll put together a plan that uses all my abilities. It's time to show the corporation that we're not just helpless."
            }}, new(){DialogOrder = new(){Quest = 6, Step = 4, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: The mission was successful. We have information in our hands that could be very damaging to the corporation.",
                "Hacktivist Leader: That is excellent news, Shadow Viper. What exactly was the information we obtained?",
                "Shadow Viper: I have obtained detailed records of the illegal activities of the corporation, including corruption, rights violations, and secret projects with unethical goals.",
                "Hacktivist Leader: This may be just the breakthrough we needed. We now have enough evidence to publicly indict the corporation and perhaps even get the authorities to act.",
                "Shadow Viper: It's enough to get all the news channels talking about it. We can use this to expose the truth and show people what's going on behind corporate walls.",
                "Hacktivist Leader: We have to be careful how we use this information. Corporations have a lot of influence and will fight back. How do you suggest we proceed?",
                "Shadow Viper: We should coordinate the leak with our allies and ensure that it reaches as wide a public as possible. At the same time, we must ensure the protection of our resources and members.",
                "Hacktivist Leader: I agree. This data can change the course of our war against corporations. Your role in this has been crucial, Shadow Viper. You are invaluable to us."
            }}, new(){Unlock = new() { Location.CorporateHeadquarters}, DialogOrder = new(){Quest = 7, Step = 1, LastStep = true}, Texts = new List<string>()
            {
                "Hacktivist Leader: Shadow Viper, we face perhaps our greatest challenge yet. We have learned of a corporate project that could profoundly affect our society. It's something we must stop at all costs.",
                "Shadow Viper: What exactly does this project involve?",
                "Hacktivist Leader: It's a new kind of technology that allows the corporation to manipulate information and surveillance on a level we've never seen before. If this project is launched, it will be a disaster for our freedom.",
                "Shadow Viper: I understand the gravity of the situation. What is your plan?",
                "Hacktivist Leader: Your mission will be to infiltrate corporate headquarters, obtain vital information about this project, and, if possible, destroy it.",
                "Shadow Viper: This will require a precise combination of all my skills.",
                "Hacktivist Leader: Exactly. This mission will combine physical infiltration work with advanced hacking. You need to be prepared for anything.",
                "Shadow Viper: It's a risk I'm prepared to take. When do we start?",
                "Hacktivist Leader: Immediately. Time is playing against us. We must act quickly and decisively before the project is launched.",
                "Shadow Viper: Understood. I won't let you down."
            }}
        }},
        {$"{Location.TacticalRoom}Dialog", new List<Dialog>()
            { new() {DialogFocus = PlayerFocus.Physics, Unlock = new() { Location.CorporalBuilding}, DialogOrder = new(){Quest = 4, Step = 2}, Texts = new List<string>
            {
                "Technical Expert: The building where they keep our member is one of the best guarded facilities in the city. We have to be smart in our approach.",
                "Shadow Viper: What are our options?",
                "Technical Expert: We have several plans. We could try a direct attack, but that would be risky and could endanger the prisoners. My suggestion is to use your new abilities for silent infiltration.",
                "Technical Expert: I have drawings of the building and the layout of the security cameras and alarms. You can get in through the roof or the basement entrance.",
                "Shadow Viper: I prefer silent infiltration. Minimal risk to prisoners and less chance of detection.",
                "Technical Expert: Excellent. Once you get in, you'll have to be quick. The security is complex and if the alarm is activated, the situation will quickly become complicated.",
                "Technical Expert: I'll equip you with a few tools to help you overcome the security. I'll also provide you with a communicator so you can report progress to us.",
                "Shadow Viper: Once I find the prisoners, how do we get out?",
                "Tech Expert: We have an escape plan in place. Once you free the prisoners, we'll direct you to the nearest safe exit.",
                "Shadow Viper: Okay, sounds like a solid plan. I'm ready."
            }}, new(){DialogFocus = PlayerFocus.Physics, Unlock = new() { Location.Facility}, DialogOrder = new(){Quest = 5, Step = 2}, Texts = new List<string>()
            {
                "Technical Expert: We have limited information about the layout and security systems of the facility, but enough to make a plan.",
                "Shadow Viper: What is the main objective of the sabotage?",
                "Technical Expert: The main objective is to shut down their central control system. That should bring the entire project to a halt and cause them considerable delay.",
                "Shadow Viper: How do I get in? And what obstacles can I expect?",
                "Technical Expert: The best way is through the side entrance they use for deliveries. It has less security. Once inside, you'll have to negotiate a series of security doors and maybe a few guards.",
                "Shadow Viper: I plan to use silent infiltration if possible. I need an exact schedule of patrols and security information.",
                "Technical Expert: We have records of their routines. I'll supply you with the exact times as well as the tools to bypass security.",
                "Shadow Viper: What plan do we have for escape",
                "Technical Expert: Once you've sabotaged the system, activate the emergency exit. We have an escape vehicle ready for you near the facility.",
                "Shadow Viper: Excellent, that sounds like a solid plan. As soon as I have all the information I need, I'll be on my way."
            }}, new(){Unlock = new() { Location.DataDepot}, DialogOrder = new(){Quest = 6, Step = 3}, Texts = new List<string>()
            {
                "Technical Expert: We have detailed information about the facility ready for you, including layout, security protocols and watch timings.",
                "Shadow Viper: Thorough information will be key. What is our plan of approach?",
                "Technical Expert: Your first task will be to get inside without being spotted by security cameras or guards. We have a map with camera blind spots and patrol timings marked.",
                "Shadow Viper: Once I'm inside, what's the next step?",
                "Tech Expert: You need to get into the central control room. That will require getting through several security doors. We'll equip you with the necessary tools to open them.",
                "Shadow Viper: What about hacking the system?",
                "Tech Expert: Once inside the control room, you'll connect directly to the central computer. We have a special hacking kit ready for you that will allow you to quickly hack into their systems.",
                "Shadow Viper: We'll have to be quick and efficient. What's our escape plan?",
                "Tech Expert: Once you gain control of the system, activate the local emergency protocol, allowing you to leave the facility without much attention. We have an escape vehicle ready nearby.",
                "Shadow Viper: Excellent, looks like we have everything we need. Let's do this."
            }}
        }},
        {$"{Location.CyberLab}Dialog", new List<Dialog>()
            { new() { DialogOrder = new(){Quest = 3, Step = 2}, Texts = new List<string>
            {
                "Ripper Dock: So you're Shadow Viper. I've heard about your abilities. I'm curious what you'll choose for your enhancements.",
                "Shadow Viper: I need something to help me deal better with the corporations. What can you offer me?",
                "Ripper Dock: We have a variety of options. We can enhance your physical abilities or expand your hacking skills. It depends on which style you prefer."
            }}, new(){ DialogFocus = PlayerFocus.Physics, DialogOrder = new(){Quest = 3, Step = 3, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: I want to focus on physical abilities. I need to get stronger and faster to face physical threats.",
                "Ripper Dock: Good choice, Shadow Viper. This upgrade will give you exactly what you need. And as for the price, you don't have to worry. Our group will cover all costs. Consider it an investment in our future together.",
                "Shadow Viper: I appreciate that. It's time to show the corporations that we're not just helpless pawns in the game.",
                "Ripper Dock: Exactly. Now sit back and let me work. You'll soon have new abilities at your disposal."
            }}, new() {DialogFocus = PlayerFocus.Hack, DialogOrder = new(){Quest = 3, Step = 3, LastStep = true}, Texts = new List<string>()
            {
                "Shadow Viper: I'm interested in hacking skills. I want to be able to hack into any system and get the information I need.",
                "Ripper Dock: Good choice, Shadow Viper. This upgrade will give you exactly what you need. And as for the price, you don't have to worry. Our group will cover all costs. Consider it an investment in our future together.",
                "Shadow Viper: I appreciate that. It's time to show the corporations that we're not just helpless pawns in the game.",
                "Ripper Dock: Exactly. Now sit back and let me work. You'll soon have new abilities at your disposal."
            }}, new(){DialogFocus = PlayerFocus.Physics, Unlock = new() { Location.TacticalRoom, Location.Workshop }, DialogOrder = new(){Quest = 6, Step = 2}, Texts = new List<string>() {
                "Ripper Dock: We have another upgrade for you, Shadow Viper. This hacking module will allow you to penetrate corporate systems more efficiently and quickly.",
                "Shadow Viper: Excellent. What's our plan for this mission?",
                "Ripper Dock: First you must physically penetrate the facility. Your physical module will help you do that. Once you're inside, your new hacking module will be next.", 
                "Shadow Viper: Sounds like a combination that might work. Once I reach the central computer, I'll use the new module to hack into their systems.",
                "Ripper Dock: Yes, and with this module you'll be able to more efficiently override their security protocols and get the information you need faster."
            }}, new(){DialogFocus = PlayerFocus.Hack, Unlock = new() { Location.TacticalRoom, Location.Workshop}, DialogOrder = new(){Quest = 6, Step = 2}, Texts = new List<string>()
            {
                "Ripper Dock: Welcome, Shadow Viper. We have another upgrade for you - a physical module. It will increase your strength and agility, allowing you to infiltrate more effectively physically.",
                "Shadow Viper: That sounds great. What's our plan for infiltrating the facility?",
                "Ripper Dock: Your mission will be physical infiltration first. You need to overcome the security guards and security devices to reach the central computer.", 
                "Shadow Viper: Once inside, I will use my hacking skills to hack into the system.",
                "Ripper Dock: Exactly. Your new physical module will allow you to quickly and silently overcome physical obstacles while your hacking module secures the digital portion of the mission."
            }}
        }},
        {$"{Location.Workshop}Dialog", new List<Dialog>()
            { new() { DialogFocus = PlayerFocus.Hack, Unlock = new() { Location.QuantumTechnology }, DialogOrder = new(){Quest = 4, Step = 2}, Texts = new List<string>
            {
                "Technical Expert: Shadow Viper, we've integrated cutting-edge quantum technology into our hacking tools, enhancing their capabilities.",
                "Shadow Viper: Quantum technology? How will that assist in this operation?",
                "Technical Expert: Quantum computing offers unprecedented processing speed and power, ideal for breaking complex encryptions and navigating sophisticated security systems.",
                "Shadow Viper: Sounds promising. Will I need special training to use it?",
                "Technical Expert: No, the interface is user-friendly. But remember, its power makes it extremely efficient, so your actions must be precise and calculated.",
                "Shadow Viper: Understood. I'll harness this quantum edge to infiltrate the database swiftly and securely."
            }}
        }},
    };
    
    private Dictionary<string, Choice> gameLocationChoiceDict = new()
    {
        {$"{Location.DarkAlley}Choice", new Choice(){ChoiceA = "Violence", ChoiceB = "Talk them Down", Description = "Choose whether to deal with the gang violently or talk them down and scare them away."}},
        {$"{Location.CyberLab}Choice", new Choice(){ChoiceA = "SKELLETRON", ChoiceB = "Brain Chip", Description = "SKELLETRON - fighting BRAIN CHIP - hacking Choose a cyberware you want to equip."}},
        {$"{Location.Boss}Choice", new Choice(){ChoiceA = "Make him suffer", ChoiceB = "Hand to Police", Description = "Choose whether to deal with the head of the corrupt corporate world with violence or let him rot in a prison."}}
        
    };

    // Default game locations' target locations states 
    private Dictionary<string, List<Location>> gameLocationTargetLocationDict = new()
    {
        //Game
        {$"{Location.SlumDistrict}TargetLocations", new List<Location> { Location.SlumQuarter, Location.DarkAlley}},
        {$"{Location.SlumQuarter}TargetLocations", new List<Location> { Location.SlumDistrict, Location.ElectroShop}},
        {$"{Location.ElectroShop}TargetLocations", new List<Location> { Location.SlumQuarter}},
        {$"{Location.DarkAlley}TargetLocations", new List<Location> { Location.SlumDistrict}},
        {$"{Location.ShadyBar}TargetLocations", new List<Location> { Location.PartOfTheBar, Location.BackEntrance}},
        {$"{Location.PartOfTheBar}TargetLocations", new List<Location> { Location.ShadyBar}},
        {$"{Location.BackEntrance}TargetLocations", new List<Location> { Location.ShadyBar, Location.SecretMeetingPlace}},
        {$"{Location.SecretMeetingPlace}TargetLocations", new List<Location> { Location.CyberLab, Location.Workshop, Location.TacticalRoom}},
        {$"{Location.Workshop}TargetLocations", new List<Location> { Location.SecretMeetingPlace, Location.QuantumTechnology}},
        {$"{Location.TacticalRoom}TargetLocations", new List<Location> { Location.SecretMeetingPlace , Location.CorporalBuilding, Location.Facility, Location.DataDepot, Location.CorporateHeadquarters}},
        {$"{Location.CyberLab}TargetLocations", new List<Location> { Location.SecretMeetingPlace}},
        {$"{Location.QuantumTechnology}TargetLocations", new List<Location> { Location.Workshop}},

        //Act1
        {$"{Location.CorporalBuilding}TargetLocations", new List<Location> { Location.MainEntrance, Location.BackOfBuilding}},
        {$"{Location.MainEntrance}TargetLocations", new List<Location> { Location.Reception}},
        {$"{Location.Reception}TargetLocations", new List<Location> { }}, // death
        {$"{Location.BackOfBuilding}TargetLocations", new List<Location> { Location.Hallway , Location.BackDoor}},
        {$"{Location.Hallway}TargetLocations", new List<Location> { Location.ServerRoom, Location.ChiefTechnitianOffice}},
        {$"{Location.ServerRoom}TargetLocations", new List<Location> { Location.SecurityDoor1, Location.ControlRoom, Location.Hallway}},
        {$"{Location.SecurityDoor1}TargetLocations", new List<Location> { Location.ServerRoom, Location.HoldingCells}},
        {$"{Location.SecurityDoor2}TargetLocations", new List<Location> { Location.Warehouse, Location.HoldingCells}},
        {$"{Location.HoldingCells}TargetLocations", new List<Location> { Location.ExtractionVan}},
        {$"{Location.ControlRoom}TargetLocations", new List<Location> { Location.ServerRoom}},
        {$"{Location.ChiefTechnitianOffice}TargetLocations", new List<Location> { Location.Hallway}},
        {$"{Location.BackDoor}TargetLocations", new List<Location> { Location.Warehouse, Location.SecurityRoom}},
        {$"{Location.Warehouse}TargetLocations", new List<Location> { Location.SecurityDoor2, Location.BoilerRoom, Location.BackDoor}},
        {$"{Location.BoilerRoom}TargetLocations", new List<Location> { Location.Warehouse}},
        {$"{Location.SecurityRoom}TargetLocations", new List<Location> { Location.BackDoor}},        
        {$"{Location.ExtractionVan}TargetLocations", new List<Location> { Location.SecretMeetingPlace}}, // end mission
        
        //Act2
        {$"{Location.Facility}TargetLocations", new List<Location> { Location.DeliveryEntrance}},
        {$"{Location.DeliveryEntrance}TargetLocations", new List<Location> {  Location.SecurityCheckpoint, Location.ToolStorage}},
        {$"{Location.SecurityCheckpoint}TargetLocations", new List<Location> {Location.ElectricalHallway, Location.DeliveryEntrance, Location.MaintenanceArea }},
        {$"{Location.ElectricalHallway}TargetLocations", new List<Location> { Location.RoomA421, Location.RoomB78, Location.SecurityCheckpoint}},
        {$"{Location.RoomA421}TargetLocations", new List<Location> { Location.ElectricalHallway }},
        {$"{Location.RoomB78}TargetLocations", new List<Location> { Location.ElectricalHallway }},
        {$"{Location.RoomA765}TargetLocations", new List<Location> { Location.MaintenanceArea}},
        {$"{Location.MaintenanceArea}TargetLocations", new List<Location> { Location.EmergencyExit, Location.RoomA765, Location.SecurityCheckpoint}},
        {$"{Location.ToolStorage}TargetLocations", new List<Location> { Location.DeliveryEntrance}},
        {$"{Location.EmergencyExit}TargetLocations", new List<Location> { Location.MaintenanceArea, Location.EscapeCar}},
        {$"{Location.EscapeCar}TargetLocations", new List<Location> { Location.SecretMeetingPlace }}, // end mission

        // Act3
        {$"{Location.DataDepot}TargetLocations", new List<Location> { Location.ThirdFloor }},
        {$"{Location.ThirdFloor}TargetLocations", new List<Location> { Location.MainLobby, Location.RooftopExit }},
        {$"{Location.MainLobby}TargetLocations", new List<Location> { Location.ThirdFloor, Location.SectorD4, Location.ResearchWing }},
        {$"{Location.SectorD4}TargetLocations", new List<Location> { Location.MainLobby, Location.Server17, Location.Server21 }},
        {$"{Location.Server17}TargetLocations", new List<Location> { Location.SectorD4 }},
        {$"{Location.Server21}TargetLocations", new List<Location> { Location.SectorD4 }},
        {$"{Location.ResearchWing}TargetLocations", new List<Location> { Location.MainLobby, Location.ComputerA765 }},
        {$"{Location.RooftopExit}TargetLocations", new List<Location> { Location.ThirdFloor, Location.Helipad }},
        {$"{Location.ComputerA765}TargetLocations", new List<Location> { Location.ResearchWing }},
        {$"{Location.Helipad}TargetLocations", new List<Location> { Location.SecretMeetingPlace }}, // end mission

        //Act4
        {$"{Location.CorporateHeadquarters}TargetLocations", new List<Location> { Location.Elevator }},
        {$"{Location.Elevator}TargetLocations", new List<Location> { Location.Penthouse }},
        {$"{Location.Penthouse}TargetLocations", new List<Location> { Location.Boss }}, // end game
        {$"{Location.Boss}TargetLocations", new List<Location> {  }} // end game


    };
 
    //Default inventory state
    private List<Item> inventoryItems = new()
    {
        //Empty inventory at the beginning
    };

    // Default quests state
    private List<Quest> questList = new()
    {
        Global.Quests.First(q => q.Number == 1),
        Global.Quests.First(q => q.Number == 3)
    };

    // Default player state
    private PlayerStats playerStats = new()
    {
        Health = 50,
        MaxHealth = 50,
        Energy = 50,
        MaxEnergy = 50,
        Money = 0,
        //TODO Default moral score
        MoralScore = 0
    };

    // Player starts without specified dealing type
    private string playerDealingType = "";

    // Game not in progress by default
    private bool gameInProgress = false;

    // Game starts at quest 1, step 1
    private GameProgress gameProgress = new() {Quest = 3, Step = 1};

    // Set default currentLocation on game start
    private Location currentLocation = Location.SlumDistrict;

    // Default equipped weapon
    private Weapon equippedWeapon = Global.Weapons.First(w => w.Type == WeaponType.Knife);
    
    public SessionHelper(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        SetSessionDefaultState();
    }

    /// <summary>
    /// Retrieves a string value from the session
    /// </summary>
    /// <param name="key"></param>
    /// <returns> session value </returns>
    public string? GetString(string key)
    {
        try
        {
            return _httpContext.HttpContext.Session.GetString(key);

        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to retrieve a value of [{key}] from the session -> {e.Message}");
        }
        
    }

    /// <summary>
    /// Stores a new key-value pair in the session
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetString(string key, string value)
    {
        try
        {
            _httpContext.HttpContext.Session.SetString(key, value);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a new pair [{key}, {value}] into the session -> {e.Message}");
        }
    }
    
    /// <summary>
    /// Retrieves a int value from the session
    /// </summary>
    /// <param name="key"></param>
    /// <returns> session value </returns>
    public int? GetInt(string key)
    {
        try
        {
            return _httpContext.HttpContext.Session.GetInt32(key);

        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to retrieve a int value of [{key}] from the session -> {e.Message}");
        }
        
    }

    /// <summary>
    /// Stores a new key-value pair in the session
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetInt(string key, int value)
    {
        try
        {
            _httpContext.HttpContext.Session.SetInt32(key, value);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a new pair [{key}, {value}] into the session -> {e.Message}");
        }
    }
    
    public void SetSessionDefaultState()
    {
        // Add game locations info
        foreach (var pair in gameLocationDataDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key.ToString(), serializedValue);
        }

        // Add dialogs
        foreach (var pair in gameLocationDialogDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }

        // Add target locations
        foreach (var pair in gameLocationTargetLocationDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }
        
        // Add game locations choices
        foreach (var pair in gameLocationChoiceDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key.ToString(), serializedValue);
        }

        // Add inventory items
        string serializedItems = JsonSerializer.Serialize(inventoryItems);
        _httpContext.HttpContext.Session.SetString("inventory", serializedItems);

        // Add quests
        string serializedQuestList = JsonSerializer.Serialize(questList);
        _httpContext.HttpContext.Session.SetString("quests", serializedQuestList);

        // Add player stats
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _httpContext.HttpContext.Session.SetString("playerStats", serializedPlayerStats);
        
        // Game not in progress
        _httpContext.HttpContext.Session.SetString("gameInProgress", gameInProgress.ToString());
        
        // Set default game progress -> used for tracking
        string serializedGameProgress = JsonSerializer.Serialize(gameProgress);
        _httpContext.HttpContext.Session.SetString("gameProgress", serializedGameProgress);
        
        // Set default currentLocation
        _httpContext.HttpContext.Session.SetString("currentLocation", currentLocation.ToString());
        
        // Set default playerDealingType
        _httpContext.HttpContext.Session.SetString("playerDealingType", playerDealingType);
        
        // Set default equipped weapon
        string serializedEquippedWeapon = JsonSerializer.Serialize(equippedWeapon);
        _httpContext.HttpContext.Session.SetString("equippedWeapon", serializedEquippedWeapon);
        
    }

}