#The203.CollectionJson

The203.CollectionJson framework provides a way to marshell your Domain model into Collection+JSON formated endpoints. The framework is young, and comes from a product that did not have other mime types, as a result it is not real strict when it comes to request types and returning CJ. CJ works with ASP MVC to create a set of endpoints that return CJ formated data. The CJ framework seeks to make mapping an application's domain to CJ spec endpoint painless and allows the application to focus on the business functions and not implementing a spec.


#Getting Started

CJ framework has 2 basic parts. Route creation and the  Collection Json Controller. The CJ Controller itself then has multiple aspects like link creation, the collection json result, and template hydration. Let's get started with Route creation.

##Route Creation

Route creation generally would go into the Global.asax. Route creation is required for link creation to work. Route creation is a fluent API to define the relationships of the different domains in an application. A domain is mapped as a set of Items and Collections and once the mapping is complete MapRoutes is called. MapRoutes saves off the routes in memory and also registers the routes with MVC's router. Items' routes are mapped to an Action Name of Item, collections' an Action Name of Collection.  Consider the following code:

```` Csharp
CollectionJsonLinker cjLinker = CollectionJsonLinker.Instance;
cjLinker.StartPrimaryRoute()
     .AddItemAndCollection<CardDeck>("Decks/{deckId}", d => d.Id)
     .AddItemAndCollection<CardDeck, Card>("Cards/{cardId}", c => c.Sequence)
     .AddEmbeddedItem<Card, Suit>("Suit")
     .MapRoutes(routes);
cjLinker.StartAlternateRoute()
     .AddEmbeddedCollection<Suit, Suit>("Suit")
     .MapRoutes(routes);
````
 
 The root collection is a Deck of cards. If a client issues a request to http://the203.info/Decks MVC will load a DecksController and run the "Collection" action. CJ does not dictate which or how an application support different HTTP verbs. Generally an application is best served supplying a meaningful name for the method and using custom attributes to denote the Action name and the HTTP verb supported. The expectation is a collection of Decks will be returned if a GET request is issued to /Decks. If a client issues a GET request to http://the203.info/Decks/123 the DecksController is loaded and the action name Item is executed. 

 The second call to AddItemAndCollection has a parent (CardDeck) and a new Collection Card. Thus these collections are now a step down on the URL. http://the203.info/Decks/123/Cards will return a collection of Cards for the Deck 123. Note that this time instead of point to an Id we send a delegate that locates off of Sequence so http://the203.info/Decks/123/Cards/3 would be expected to return the 3rd card in the deck. 

 Embedded Items are intended to only map to a single item and generally that item is also a top level item elsewhere in the system. Embedded items are sort of a weird beast and need to be fixed up a little and documented better, look at the tests if you want to know them more

##Collection Json Controller
To take advantage of marshaling your domain into CJ format and a few other things your MVC controllers should extend the CollectionJsonController. Once an application's controller has extended from the CJ Controller the application can take advantage of a few things in its controllers. It can create CollectionJsonResult, map out the links or hydrate objects from collection json or other input. 

###Returning CJ Formatted Data
 This follows MVCs concept of calling View only an application makes a call to CollectionJsonResult<T>  a single instance of T or an IEnumerable of T can be sent into this method. Returning the result of the method call will result in a  very simple CJ formatted object. Generally this is not enough and the application will want a links section as well. 

###Link Creation
 Right now link creation is required in each CJResult creation. I'm not a big fan of this and thanks to some work that has been done inside the link creation I think this might change in the future. For now this is how it works. 

The links section is created by calling BuildLinks on the CJResult. This provides a sort of fluent API to build out the links section of the CJ return. The link builder will detect items that do not exist and only provide links to those items that actually exist. This makes building links much more elegant by not requiring a liteny of if statements breaking up the fluent api calls. Most methods have an Always varient (or should) that denote a link should be created even if the acessor returns a nothing. Consider this code block: 
````Csharp
CollectionJsonResult<CardDeck> cjr = CollectionJsonResult<CardDeck>(deck);
cjr.BuildLinks()
	 .IsParent()
	 .AddChildCollection<Card>("Cards", d => d.Cards)
	 .AddRelativeGroup<Player>("Player", d => d, "Players");
````

The CardDeck is the root collection so IsParent is called to denote, there is no need to crawl an object graph somewhere to find the root collection. AddChildCollection will add the Cards link to the Link section and the link builder will check deck.Cards to ensure it is a valid link for the Deck object. The Deck also has Players and those players' profiles might be available at another defined root element (which why it is a RelativeGroup). 

###Template Hydration

Again template hydration tries to follow along with MVC on hydrating an object. Template Hydration takes a Template object which can be a domain object -or- simplified object and it will hydrate the object based off the body with CJ formatted JSON, it will also hydrate based off forms and querystring. This allows an application to consistently rely on CJs Hydration system regardless of input techniques (templates or queries for example). Template hydration is CORS compliant as well and does deal with IE 8 and IE 9's shortcomings with CORS and POSTs. 







