# README

Um einen Character zu erstellen (Gegner oder npc) muss man nur Die komponente 
Character,
weaponSystem(wenn gegner),
Enemy AI und 
Health System(wenngegner)
hinzufügen.

Einstellungen sind größtenteils selbsterklärend. 

IMMER AUF DEN KLEINEN PUNKT RECHTS NEBEN DEM FELD DRÜCKEN UM EIN OBJECT WIE EINEN CHARACTER ANIMATOR HINZUZUFÜGEN. 
Oder man geht in den jeweiligen ordner und zieht es ins Feld rein.

# 1. CHARACTER SCRIPT:

# 1.1 Animator:
Character braucht ZWANGSWEISE! einen animator (Den Character Animator) und einen sogenannten override controller. (Der jewileige override controller sollte dem namen des gegners entsprechen z.B. Archer hat den Archer Override.
Der Avatar ist wie beim override controller der Name + Avatar. z.b. Archer hat den archerAvatar.

# 1.2 Audio:
Audio feld ist erstmal nicht weiter wichtig, ist der smoothe übergang wenn man aus der range des soundeffekts tritt....

# 1.3 Capsule Collider:
Capsule collider sind die Hitbox Einstellungen

# 1.4 Movement:
Stopping distance:
ist die distance die der character zum spieler einhält.

MoveSpeed multiplier:
meist auf wert 1.2 gelassen, hab festgestellt das es am besten passt. Was genau das ist kann man rumprobieren.(Hat was damit zu tun wie schnell er von 0 auf 100 kommt sozusagen :D    )

Moving Turn speed:
Wie schnell dreht sich der character wenn er läuft.

Stationary turn speed:
Wie schnell dreht sich der character wenn er steht.

Move Threshhold:
ist erstmal nicht weiter wichtig kann ich auch als hard coded wert eintragen. 1 ist perfekt

# 1.5 NavMeshAgent:
Speed:
Wie schnell soll sich der Character während dem pathfinding bewegen

Nav Mesh Stopping Distance:
Wie weit von dem Ziel soll der Char stehen bleiben.

Nav Mesh Base Offset:
Die hitbox des Pathfindings. Kann so bei 0 - 0.3 liegen für das beste Ergebniss.
________________________________________________________________________________________________________________________________________
________________________________________________________________________________________________________________________________________
________________________________________________________________________________________________________________________________________
________________________________________________________________________________________________________________________________________
# 2.0 WeaponSystem
Dieses Script wird nur auf einem Gegner gebraucht.
Im WeaponSystem befindet sich alles was mit der waffe zu tun hat. (Crit, Dmg etc..)

Base Damage:
eigl. selbsterklärend. wieviel dmg macht die Waffe.

Current WeaponConfig:

Dort kann ein sogenanntes ASSET hinzugefügt werden. 
Im ordner Assets/Characters/Weapons
und dann ein unterordner deiner wahl, ist eine Waffe enthalten (Oder verschiedene).
Du kannst hier z.b. in den Ordner Melee Weapons und dann unter Dagger gehen.
Dort segen wir das sogenannte dagger Prefab ganz links. (Ist das object selber mit allen einstellungen etc.)
Danaben ist das Asset was letztendlich auf die weaponconfig gezogen werden muss.
(Assets kannst du auch selber erstellen. Dafür habe ich ein Script vorbereitet. Mach einfach rechtsklick Create/RPG/Weapon
und erstelle ein neues.)
Dort verarbeitet mein Script sachen wie den 

Grip Transform:
Der ist dafür da um dem object zu sagen wie es vom spieler gehalten werden soll.

möchtest du ein neues machen Ziehe einfach den Dagger in die Hierarchy (ganz link wo alle objects enthalten sind)
drücke einmal "F" um deine sicht darauf zu focussieren wenn dies nicht automatisch geschieht.
Jetzt zieh den dagger zur spieler Hand (rechts ist die waffenhand) und
jetzt rotiere (wenn man das objekt anwählt und "E" drückt) bis der dagger passt so wie er ihn halten soll.
NUN Aufpassen :D
Oben rechts in der Ecke befindet sich die Componente Transform Gehe dort auf das kleine Zahnrad und auf copy component.
Jetzt geh rechts in die hierarchy und mache rechtsklick, dann auf "Create empty"
Das erstellte objekt hat auch ein Transform, gehe dort auf das zahnrad und auf Paste Component Values.
Jetzt kannst du das neu erstellte objekt mit den transform informationen vom dagger einfach unten zu den anderen files ziehen und du hast dein Grip Transform.

WeaponPrefab:
Einfach das object des Daggers (Ganz links) hineinziehen.

Attack Animation:
Dort kannst du einfach rechts auf den kleinen punkt gehen und eine von vielen animationen auswählen

Max AttackRange:
Ist die reichweite der waffe.

Time between hits:
Wie lange soll der character pro hit wartenm (Attackspeed nur umgedreht :D)

Additional Damage:
Wieviel dmg macht die Waffe.

DamageDelay:
nach wieviel sekunden soll der damage kommen? Am besten so einstellen das der damage halt während der animation kommt und es so aussieht als würde er treffen.

SOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
zurück zur WeaponConfig auf deinem Character.

Dort haben wir lediglich nur noch Crit Chance und multiplier (Ist selbsterklärend oder?)

# 3.0 HealthSystem
Dieses Script wird nur auf einem Gegner gebraucht.

Max HealthPoints:
Wieviel leben soll der gegner haben

HealthBar:
Einstellbar für jeden Character (Lediglich das Bild der Healthbar, könnten auch für alle die gleiche nutzen)

Damage Sounds:
Dies habe ich in ein sogenanntes Array gemacht. Die Größe ist flexibel und die sounds werden Random genommen

DeathSounds:
siehe Damage Sounds

DeathVanishSeconds:
Wie lange soll der char noch bleiben bevor er gelöscht wird nach dem tod.

# 4.0 EnemyAI

Chase Radius:
Wie groß ist der Radius in dem dich der gegner verfolgt?

PatrolPath:
Hier wirds tricky.
Du kannst jedem Gegner einen PatrolPath geben wenn du das möchtest.
Diesen erstellst du am besten und dem Punkt PatrolPaths in der Hierarchy Links. (Da sind schon welche drin)
Du kannst einen bestehenden nehmen und STRG + D drücken um ihn zu duipplizieren.
Die kannst dann in der welt die punkte sehen in dem du den dupplizierten ein wenig verschiebst.
Wenn du links auf den grauen Pfeil gehst (In der hierarchy unter deinem dupplizierten PatrolPath) siehst du die "Kinder".
Namens Waypoint(0) usw. 
Diese kannst du individuell verschieben oder einfach neue hinzufügen um den controlPath des gegners festzulegen
Wichtig ist das das "Parent" objekt also in dem fall namens PatrolPath (...) mein script Waypoint Container als componennte hat.
Wenn du dann deinen PatrolPath fertig eingerichtet haben solltest dann zieh ihn einfach auf den Unterpunkt Patrol Path auf dem
EnemyAI script auf dem character den du erstellen möchtest.

WaypointTolerance:
ist die Distanz zwischen den Punkten (Kann dafür verantwortlich sein das er nicht patrolliert, nimm dann einfach einen höheren wert).

WayPointMoveTime:
Die zeit die er pro Waypoint wartet.

# Hoffe du hast es bis hier alles verstanden ^^ sorry für den langen Text.

