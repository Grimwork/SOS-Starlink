# SOS Starlink
###### Fait par "Enzo Rudy Sekkai" et Grimwork

![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)

### Contexte :

Suite à un incident, une zone accidentée a perdu ses lignes de télécommunications et donc ses accès au réseau. Pour offrir un accès réseau au secours et à la population sur place, un essaim de drone est déployé au-dessus de la zone en guise de relais réseau.

### Objectifs :

Les drones seront utilisés pour rétablir la couverture de la zone et fournir des informations sur la situation sur place. Dans un premier temps, les zones les plus critiques seront identifiées afin d’être prioritaires pour une couverture réseau. Ensuite, un recouvrement optimal et robuste est calculé par la station au sol avant d’envoyer l’essaim de drone recouvrir la zone.

Pour résumé :
- Reconnaissance du terrain
- Recouvrement optimal de la zone
- Robustesse en cas de déconnexion d’un drone

### Modélisation

Pour réaliser ce projet nous avons simuler cela à l'aide de Unity.

Dans cette simulation nous avons choisis de représenter le système à l'aide de 4 drone qui émettent un réseau aux habitations qu'ils survolent. Ces derniers ont également à charge la reconnaissance du terrain à l'aide de caméra embarqué.
10 autres drone ont eux à charge de faire un relais réseau de la station au sol jusqu'à la zone critique identifié.

Sur la figure ci-dessous nous pouvons remarquer les deux types de drones.

![Les deux types de drones](/img/EssembleDesDronesSurUnity.PNG "Les deux types de drones")
