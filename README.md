# MusicianDB

A Web API project I developed for learning ASP.NET Core.
The database contains informations on musicians, bands, concerts and venues.
The goal was to create methods that would serve bands, musicians, organizers or listeners accessing the database.

Project contains RESTful endpoints that could serve various purposes.
For example:
  - Using GetArtistsWithoutBands method from Artist service, bands can find available musicians that they can collaborate with
    or they can use GetInstrumentalists method from Instrument service to search for musicians playing a specific instrument.
  - Using GetAvailable method from Band service, organizers can find bands that are available in a certain date for their events.
    They can also specify a genre if they like.
  - Followers of a band can use GetBandConcerts method from Band to find their favorite band's concert schedule between specified dates.


## Implemented Technologies and Techniques

This project was developed using various technologies and software development techniques to create a robust and efficient web API. Some of the key technologies and techniques applied include:

- **ASP.NET Core 6:** The foundation of the project, providing a powerful framework for building web APIs.

- **Entity Framework Core:** Used for database access and management, simplifying data operations.

- **Fluent Validation:** Implemented for robust and flexible input validation, ensuring data integrity.

- **Mapping:** Employed to efficiently transform data between different layers of the application.

- **Layered Architecture:** A structured and organized approach to code organization, enhancing maintainability and scalability.

- **Caching:** Utilized to optimize performance by storing frequently accessed data in memory.

- **Authorization Mechanism:** Implemented to enhance security, including password hashing and salting for user authentication.

This combination of technologies and techniques results in a well-rounded and versatile Musician Database web API, capable of serving various users and use cases in the live music sector.
