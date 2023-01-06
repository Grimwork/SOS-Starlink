# SOS Starlink
###### Fait par EnzoRudySEKKAI et Grimwork

![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)

Vidéo démo : https://youtu.be/d7H134kagGE 

### Contexte :

Suite à un incident, une zone accidentée a perdu ses lignes de télécommunications et donc ses accès au réseau. Pour offrir un accès réseau au secours et à la population sur place, un essaim de drone est déployé au-dessus de la zone en guise de relais réseau.

### Objectifs :

Les drones seront utilisés pour rétablir la couverture de la zone et fournir des informations sur la situation sur place. Dans un premier temps, les zones les plus critiques seront identifiées afin d’être prioritaires pour une couverture réseau. Ensuite, un recouvrement optimal et robuste est calculé par la station au sol avant d’envoyer l’essaim de drone recouvrir la zone.

Pour résumé :
- Reconnaissance du terrain
- Recouvrement optimal de la zone
- Robustesse en cas de déconnexion d’un drone

### Modélisation

Pour réaliser ce projet nous avons simulé cela à l'aide de Unity.

Dans cette simulation nous avons choisi de représenter le système à l'aide de 4 drones qui émettent un réseau aux habitations qu'ils survolent. Ces derniers ont également à charge la reconnaissance du terrain à l'aide de caméras embarqués.
10 autres drone ont eux, à charge de faire un relais réseau de la station au sol jusqu'à la zone critique identifié.

Sur la figure ci-dessous nous pouvons remarquer les deux types de drones, ainsi que la modélisation de la ville.
![Les deux types de drones](/img/EssembleDesDronesSurUnity.PNG "Les deux types de drones")

<p float="left">
	<img src="/img/planVille1.PNG" alt="Ville1" width="450"/>
	<img src="/img/planVille2.PNG" alt="VillePlanDessus" width="350"/>
</p>

Comme nous pouvons le voir les différents batiments de la ville ont des couleurs associées. Ces couleurs représente l'importance qu'ils representent.

- Vert -> Low
- Cyan -> Medium
- Rouge -> High
- Violet  -> Very High

 Lorsque l'on lance la simulation une zone critique aléatoire est choisie dans la ville. Elle est représentée par un carrée ou rectangle rouge.
 
<img src="/img/zoneCritique.PNG" alt="ZoneCritique" width="500"/>

Pour modéliser le champ d'action des drones nous avons choisi d'utiliser les "SphereCollider" fourni par Unity. Sachant que la simulation est à échelle réelle, le rayon de ces derniers est de 300 mètres comme spécifier dans le cahier des charges. Le carré visible sous eux est la représentation de ce que les caméras percoivent.

<img src="/img/colliderDrone.PNG" alt="colliderDrone" width="350"/>

### Algorithme d'optimisation de la répartition des drones

Dans un premier temps pour répartir les drones de manière optimisé, il faut savoir si nous pouvons la recouvrir entièrement. Sachant que nous connaisssons la taille de la zone en avance il suffit alors de calculer l'aire de la zone et de la comparer à l'aire additionnée de la couverture réseau de nos drones. 

Si c'est le cas alors nous pouvons découper la zone en fonction du nombre de drones à disposition et les envoyer aux centres de ces nouvelle zones.

Exemple avec 4 drones :

<img src="/img/exemple1.PNG" alt="exemple1" width="350"/>

Sinon la solution proposée est la suivante :

- Division de la zone avec le nombre de drone à disposition
- Somme de chacune d'entre elles en fonction de l'importance des batiments
- Déploiment de drone jusqu'à la zone avec la plus grosse sommes
    - Si tout les drones ont été utilisé alors Stop
	- Sinon s'il reste des drones alors :
	    - Déploiment de drone jusqu'à la seconde zone la plus importante
		- Refaire jusqu'à que le nombre de drone restant = 0

A savoir que lorsqu'un drone est désactivé pour une certaine raison durant leurs déploiements alors nous relançons l'algorithme décris ci dessus avec le nouveau nombre de drone.

Représentation de l'algorithme avec 8 drones :

<p float="left">
	<img src="/img/exemple2_1.PNG" alt="exemple2_1" width="350"/>
	<img src="/img/exemple2_2.PNG" alt="exemple2_2" width="400"/>
</p>
