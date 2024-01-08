# PT_Demo_MongoDB

In addition to PT_Demo_MongoDB, you can see MongoDB used for Database Caching in [PT_Demo_CacheTower](https://github.com/petartotev/PT_Demo_CacheTower#database-caching).

## Contents
- [Initial Setup](#initial-setup)
- [Seed Data in MongoDB Database](#seed-data-in-mongodb-database)
- [.NET Web API using CRUD with MongoDB](#net-web-api-using-crud-with-mongodb)
- [Teardown](#teardown)
- [Links](#links)

## Initial Setup

1. Pull latest `MongoDB` Docker Image and run `MongoDB` in a Docker Container:

```
docker pull mongo:latest
docker run --name mongodb -p 27017:27017 -d mongo:latest
```

2. Download [MongoDB CLI](https://www.mongodb.com/try/download/shell):

```
mongosh-2.1.1-x64.msi
```

3. Install MongoDB Shell to:

```
C:\Program Files\mongosh
```

4. Run `C:\Program Files\mongosh\mongosh.exe` or execute command `mongosh` in `cmd.exe`.<br>Next, use the default connection string:

```
C:\Users\USER>mongosh
```

Output:
```
Current Mongosh Log ID: 6599af407a335d840b945ef5
Connecting to:          mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.1.1
Using MongoDB:          7.0.4
Using Mongosh:          2.1.1

For mongosh info see: https://docs.mongodb.com/mongodb-shell/

------
   The server generated these startup warnings when booting
   2024-01-06T19:50:35.891+00:00: Using the XFS filesystem is strongly recommended with the WiredTiger storage engine. See http://dochub.mongodb.org/core/prodnotes-filesystem
   2024-01-06T19:50:36.532+00:00: Access control is not enabled for the database. Read and write access to data and configuration is unrestricted
   2024-01-06T19:50:36.532+00:00: /sys/kernel/mm/transparent_hugepage/enabled is 'always'. We suggest setting it to 'never'
   2024-01-06T19:50:36.532+00:00: vm.max_map_count is too low
------
```

## Seed Data in MongoDB Database

0. Run `C:\Program Files\mongosh\mongosh.exe` or execute command `mongosh` in `cmd.exe`, then use the default connection string *(see previous section)*.

1. Create new Database `School`:

```
test> use School
```

Output
```
switched to db School
```

2. Insert many `Students`:

```
School> db.Students.insertMany([
...     { "FirstName": "Petar", "LastName": "Totev", "Age": 34, "City": "Burgas" },
...     { "FirstName": "John", "LastName": "Doe", "Age": 20, "City": "New York" },
...     { "FirstName": "Iggy", "LastName": "Pop", "Age": 77, "City": "Los Angeles" }
... ])
```

Output:
```
{
  acknowledged: true,
  insertedIds: {
    '0': ObjectId('6599afe27a335d840b945ef6'),
    '1': ObjectId('6599afe27a335d840b945ef7'),
    '2': ObjectId('6599afe27a335d840b945ef8')
  }
}
```

3. Insert single `Student`:

```
School> db.Students.insertOne({ "FirstName": "Santa", "LastName": "Claus", "Age": 101, "City": "Lapland" });

```

Output:
```
{
  acknowledged: true,
  insertedId: ObjectId('6599b0037a335d840b945ef9')
}
```

4. Update single `Student`:

```
School> db.Students.updateOne({ "_id": ObjectId("6599b0037a335d840b945ef9") }, { $set: { "Age": 99 } });
```

Output:
```
{
  acknowledged: true,
  insertedId: null,
  matchedCount: 1,
  modifiedCount: 1,
  upsertedCount: 0
}
```

5. Get all `Students`:

```
School> db.Students.find();
```

Output:
```
[
  {
    _id: ObjectId('6599afe27a335d840b945ef6'),
    FirstName: 'Petar',
    LastName: 'Totev',
    Age: 34,
    City: 'Burgas'
  },
  {
    _id: ObjectId('6599afe27a335d840b945ef7'),
    FirstName: 'John',
    LastName: 'Doe',
    Age: 20,
    City: 'New York'
  },
  {
    _id: ObjectId('6599afe27a335d840b945ef8'),
    FirstName: 'Iggy',
    Pop: 'Doe',
    Age: 77,
    City: 'Los Angeles'
  },
  {
    _id: ObjectId('6599b0037a335d840b945ef9'),
    FirstName: 'Santa',
    LastName: 'Claus',
    Age: 99,
    City: 'Lapland'
  }
]
```

6. Get `Student` by Id:

```
School> db.Students.findOne({ "_id": ObjectId('6599b0037a335d840b945ef9') });
```

Output:
```
{
  _id: ObjectId('6599b0037a335d840b945ef9'),
  FirstName: 'Santa',
  LastName: 'Claus',
  Age: 99,
  City: 'Lapland'
}
```

7. Delete `Student` by Id:
```
db.Students.deleteOne({ "_id": ObjectId('6599b0037a335d840b945ef9') });
```

Output:
```
{ acknowledged: true, deletedCount: 1 }
```

8. Get all `Students` after you deleted `Santa Claus`:

```
School> db.Students.find()
```

Output:
```
[
  {
    _id: ObjectId('6599afe27a335d840b945ef6'),
    FirstName: 'Petar',
    LastName: 'Totev',
    Age: 34,
    City: 'Burgas'
  },
  {
    _id: ObjectId('6599afe27a335d840b945ef7'),
    FirstName: 'John',
    LastName: 'Doe',
    Age: 20,
    City: 'New York'
  },
  {
    _id: ObjectId('6599afe27a335d840b945ef8'),
    FirstName: 'Iggy',
    Pop: 'Doe',
    Age: 77,
    City: 'Los Angeles'
  }
]
```

9. Get all `Students` where Age is greater than 30:

```
db.Students.find({ "Age": { $gt: 40 } });
```

Output:
```
[
  {
    _id: ObjectId('6599afe27a335d840b945ef8'),
    FirstName: 'Iggy',
    Pop: 'Doe',
    Age: 77,
    City: 'Los Angeles'
  }
]
```

## .NET Web API using CRUD with MongoDB

0. Make sure you did all steps described in [Initial Setup](#initial-setup) and [Seed Data in MongoDB Database](#seed-data-in-mongodb-database) sections and you have some `Student` entities seeded!

0. Create new blank .NET Solution `PT_Demo_MongoDB` and add new .NET 6 Web API project `DemoMongoDb.API`.

1. Install the [MongoDB.Driver NuGet package](https://www.nuget.org/packages/MongoDB.Driver):

```
dotnet add package MongoDB.Driver --version 2.23.1
```

3. Implement the `MongoDbController.cs`:

```
check repo
```

## Teardown

1. Remove the School database by executing the following command in the `mongosh`:

```
use School
db.dropDatabase()
```

Output:
```
already on db School
{ ok: 1, dropped: 'School' }
```

## Links
- https://www.mongodb.com/
- https://www.mongodb.com/try/download/shell
- https://stackoverflow.com/questions/26868367/cannot-deserialize-string-from-bsontype-objectid-in-mongodb-c-sharp