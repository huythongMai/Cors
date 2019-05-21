# Cors

Http Error 405
https://forums.asp.net/t/2105198.aspx?WEB+API+405+ON+POST+Method

Bonnes pratiques et règles de nommage
REST n’est qu’un style d’architecture, il n’y a donc aucune contrainte quant à la façon d’écrire votre code. Malgré tout, l’état de l’art nous apporte un certain nombre de bonnes pratiques pour concevoir une API REST efficace et élégante :

URL
Privilégier l’utilisation de nom (pour désigner les ressources). Les verbes peuvent tout de même avoir leur utilité lorsque l’on souhaite désigner une action ou une opération
/users
/books/5/download
Généralement, le standard privilégie l’usage du pluriel  (pour intégrer l’ensemble des routes traitant d’un même sujet sous le même préfixe). Le singulier peut malgré tout être utile dans les cas où l’entité est unique et qu’on souhaite l’indiquer de manière claire
Pour la casse, le lowercase ou le snake_case est préférable, tout en gardant à l’esprit que le plus important est d’utiliser la même partout !
Paramètres 
Path : pour les paramètres obligatoires ou des identifiants de ressources
/books/5
Query : pour les paramètres optionnels
/books/5?view=compact
Body : pour les données à envoyer
Header : pour les paramètres globaux à l’ensemble de l’API (token d’authentification, numéro de version…)
Status HTTP
1xx : indique au client qu’il doit attendre quelque chose
2xx : indique au client un succès et lui fourni la réponse attendue
3xx : indique au client un problème d’emplacement (redirection)
4xx : indique au client qu’il a fait une erreur
400 (Bad Request) : erreur de syntaxe
401 (Unauthorized) : il est nécessaire d’être authentifié pour accéder à cette ressource
403 (Forbidden) : la requête est valide mais refusée par le serveur
404 (Not Found) : la ressource n’existe pas ou n’a pas été trouvée
405 (Not Allowed) : le verbe HTTP demandé n’est pas autorisé pour cette ressource
422 (Unprocessable Entity) : indique que l’entité fournie en paramètre est incorrecte (très pratique pour gérer les erreurs fonctionnelles)
…
5xx : indique au client que le serveur a fait une erreur
500 (Internal Server Error) : erreur interne du serveur
501 (Not Implemented) : la fonctionnalité demandée n’est pas supportée par le serveur
503 (Service Unavailable) : service temporairement indisponible ou en maintenance
Avec tout ça, il n’est donc plus question de renvoyer un code 2xx avec une entité contenant un bloc d’erreur !
Gestion des erreurs : les statuts HTTP fournissent une gestion d’erreur intéressante, mais pouvant manquer de granularité et de précision. Il n’existe malheureusement pas de standard à ce sujet et il est donc nécessaire de définir son propre format d’échange.
Filtrage des données :
Objectifs :
Réduire le volume de données à échanger ou le nombre de requêtes entre client et serveur
Cibler des plateformes différentes : standards (sites web, BackOffice…) ou mobiles
Solutions :
En spécifiant explicitement la liste des champs que l’on souhaite récupérer
/books?order=up&filter=title,author,year
Via un style que l’on aura défini préalablement (minimal, mobile, compact…)
Versioning
Le versioning va principalement intervenir dans le cas où le besoin change et qu’il est nécessaire de faire évoluer la signature du service. Dans ce cas, soit la compatibilité descendante est conservée (c’est-à-dire que les utilisateurs actuels de l’API ne seront pas impactés par les changements), soit elle ne l’est pas. Dans ce dernier cas, il est donc nécessaire de versionner notre API. Il y a plusieurs façons de faire :

Via l’URL :
Soit directement dans le path (/api/v1/myEndpoint) ou avec un paramètre (/api/myEndpoint?version=1)
(-) Technique simple et pratique à mettre en place mais qui pose 2 soucis :
on expose un paramètre technique dans l’URI
on rompt avec les principes de REST, et notamment le fait qu’une ressource n’est plus identifiée par une seule et unique URL. De plus, cela peut faire croire à l’utilisateur qu’il existe une ressource différente par version alors que c’est simplement sa représentation (son format) qui change.
(+) Permet de partager facilement les liens vers l’API (mail, Twitter…)
Dans la mesure du possible, méthode à éviter, car cela impose de modifier les URLs
Via le nom de domaine :
https://v1.myapi.com/myEndpoint/
Via un en-tête HTTP personnalisé :
api-version: 2 ou X-API-VERSION: 2
Dans ce cas, l’url n’est pas modifiée, mais on ajoute un en-tête HTTP personnalisé
(+) Simple à mettre en place côté client
(-) Redondant par rapport à l’en-tête HTTP standard « Accept »
Via l’en-tête HTTP « Accept » :
Accept: application/[app].[version]+[format]
[app] le nom de l’application
[version] le numéro de version
[format] format de retour désiré (ex. JSON)
Ce qui nous donne par exemple : Accept: application/myCustomApi.v2+json
Repose sur ce que l’on appelle la « négociation de contenu » (via un type MIME)
(+) Méthode élégante et parfaitement conforme à l’esprit REST
(+) Il est possible pour le client de demander plusieurs formats, avec un ordre de préférence
(-) Complique beaucoup les tests 🙂
C’est le client qui décide quelle version du contenu il souhaite et c’est le mieux placé pour savoir quel format et quelle version il est capable de gérer 🙂