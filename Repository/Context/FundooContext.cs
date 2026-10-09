using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Context
{
    public class FundooContext: DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<PasswordResetTokenEntity> PasswordResetTokens { get; set; } = null!;

    }
}

// why different folder
// what if i create mysql, postgresql and other other db



// connection with db
// Dbset - connect - class with users table ehich is inside db
// if table not exist -  it will create it
// user entity ke andr vhi sab hona chaiye jo user table mei hai



// --
//two approach

// approach1 - code first, called data migration
// create entity class instad of table
// use this class to create table automatically inside db
// eg - 

// approach2 - database first, scan folding
// database ki har ek table ko dekh kr , khud se entities bnao
// or
// ef ko db  provide krenge and it will automatically creates entity  according to data


// when to use what

//| Situation | Usually use | Why |
//| --------------------------------------------------- | --------------------------------------- | ------------------------------------------------------ |
//| New application, team owns both API + DB            | **Code First**                          | Easy to evolve schema with application code            |
//| Existing database already designed                  | **Database First**                      | Generate entities from the existing schema             |
//| Company has a DBA who controls DB changes           | **Database First** or DB-managed schema | Database is the source of truth                        |
//| Small/medium application where developers manage DB | **Code First + Migrations**             | Convenient and keeps schema changes version-controlled |
//| Legacy database                                     | **Database First**                      | Avoid rebuilding/redesigning an existing DB            |
//| Microservice with its own database                  | Often **Code First + migrations**       | Service can independently manage its schema            |
