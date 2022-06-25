# Projet_RV_Golf

Développement d'un jeu de golf sur desktop réalisé par [Yoann PERIQUOI](https://gitlab.iut-clermont.uca.fr/yoperiquoi) et [Jovann BORDEAU](https://gitlab.iut-clermont.uca.fr/jobordeau) dans le cadre du cours de RV en 2ème année de DUT Informatique supervisé par l'intervenant [M. Nicolas Raymond](https://gitlab.iut-clermont.uca.fr/niraymon).

https://user-images.githubusercontent.com/74385376/175613161-b7146bfc-40e6-45ec-9396-206df002189e.mp4

## Contexte de l'application

Dans le cadre de notre matière de réalité virtuelle nous avons eu à réaliser une jeu vidéo en duo en 7 semaines grâce à un Framework de développement de jeu vidéo en C#.NET.

Avez vous déjà réver d'évoluer dans le monde du golf professionnel ? Alors nous avons une solution pour vous !
Nous avons développé un jeu nommé MiniGolf. Celui-ci est un jeu de golf en 3D. Il a en particulier été développé en C# avec le framework de développement de jeux vidéos MonoGame. MiniGolf est comme son nom le suggère un jeu de mini golf dans lequel vous devez envoyez la balle dans un trou le tout avec le moins de coups possible. Seulement, cela n'est pas simple puisque les parcours sont jonchés d'un grand nombre d'obstacles. Châteaux et trous béants serront des obstacles qui ralentiront votre avancée.

Comme dis juste avant, nous avons fais le choix de développer une application en 3D. Seulement le Framework Monogame n'est pas forcément l'outils idéal pour développer ce genre d'application. C'est pourquoi, nous avons eu beaucoup de fil à retordre pour réaliser ce jeu, surtout au niveau de la détection des collisions.

Pour réaliser la collision, nous avons tout d'abord demandé de l'aide à M.Nicolas Raymond, intervenant au DUT Informatique de Clermont-Ferrand, afin de développer cette collision. Pendant un grand nombre de semaines, nous étions donc à la recherche de la méthode nécessaire à la détection des *Bounding Box* ou bordure du terrain. Seulement, cette recherche n'a pas été fructueuse et malgrès les cours passé à collaborer avec M.Raymond pour essayer de trouver une solution nous ne sommes pas arrivé à un résultat viable. C'est pour cela que rendu à la semaine 7 nous n'avions pas de collision et nous avons décider d'utiliser une bibliothéque de simulation de physique en C# appelé [BepuPhysic](https://github.com/bepu/bepuphysics1).

Cette bibliothèque nous as donc permis de mettre en place une gravité ainsi que les collisions dans notre jeu.

Par la suite, pour réaliser notre interface nous avons choisi d'utiliser la bibliothèque [Apos.GUI](https://github.com/Apostolique/Apos.Gui). Celle-ci nous as permis de faire des interfaces efficacement.

## Déploiement

Pour déployer notre application il suffit juste de se rendre dans le fichier [Release](/Release/Golf.Windows_1.0.1.0_Debug_Test) de notre application dans le fichier Golf et enfin de faire clique droit sur le fichier *install.ps1* et enfin d'appuyer sur *Exécuter avec Powershell*. Cela installera automatique l'application sur votre ordinateur et vous n'aurait plus qu'a chercher et exécuter l'application dans votre navigateur Windows qui se nomme MiniGolf.

## Jouer à MiniGolf

Pour jouer à MiniGolf il n'y a rien de plus simple. En effet, après avoir lancé l'application, il suffit juste de cliquer sur *Launch Game*. Le terrain et la balle apparaît et vous pouvez désormais commencer à jouer. Pour tapper dans la balle il n'y a rien de plus simple. Comme dans la vrai vie, la balle se dirige vers où vous tappez ou dans notre cas, là où vous regardez. La direction du coup suit la direction vers laquelle la caméra est pointé. Par la suite, pour doser la puissance mise dans la balle, vous devrait faire un clic plus ou moins long afin de dicter la puissance au coup. Pour vous aider une jauge apparaîtra en bas à droite de l'écran et vous indiquera la puissance que sera actuellement mise dans la balle. Attention quand on a commencé à cliquer, il n'y a plus de marche arrière.

**A noter que si vous tombez vous réapparissez au point de départ et si jamais vous êtes bloqué vous pouvez appuyer sur la touche *R* pour revenir au point de départ également**

L'objectif est bien évidemment de terminer tout les trous avec le moins de coups possible.

Un tableau des score vous sera afficher à la fin de la partie vous récapitulant tout vos scores pour chaques niveaux.


## Documentation 

L'intégraliter de la documentation du projet est retrouvable dans le dossier [Documentation](/Documentation).

## Manques dû au retards 

Comme cité plus haut, la collision est un élément sur lequel nous nous sommes attardé et qui nous pris un grand nombre d'heures. Ce retard nous à pousser à commencer la logique pur de l'application bien trop tard. C'est pour ça que le jeu est pour le moment seulement en mode joueur unique. Seulement comme nous avons dès le départ l'idée de proposer un mode multijoueur local, des points d'extensions on été mis en place afin de simplifier son implémentation prochaine. De plus, nous voulions proposer notre application sur un grand nombre de plateformes. Seulement, pour l'instant MiniGolf est disponible uniquement en application de bureau Windows. Nous avons donc essayer de fournir un jeu le plus complet possible en très peu de temps.

## Remerciements

Nous remercions M. Nicolas Raymond pour le grand nombre d'heures que celui-ci à passer avec nous en cours et en dehors afin de trouver une solution à la recherche d'un moyen de détection des collisions ainsi que pour le suivi général du projet.


