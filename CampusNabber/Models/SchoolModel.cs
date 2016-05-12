﻿using DatabaseCode.CNObjectFolder;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace CampusNabber.Models
{
    public class SchoolModel
    {

        public System.Guid object_id { get; set; }
        public string school_name { get; set; }
        public string address { get; set; }
        public string main_hex_color { get; set; }
        public string secondary_hex_color { get; set; }
        public string school_tag { get; set; }

        public SchoolModel()
        {

        }

        public School bindSchool()
        {
            School school = new School();
            school.object_id = this.object_id;
            school.school_name = this.school_name;
            school.address = this.address;
            school.main_hex_color = this.main_hex_color;
            school.secondary_hex_color = this.secondary_hex_color;
            school.school_tag = this.school_tag;
            return school;
        }

        public SchoolModel bindToSchoolModel(School school)
        {
            return new SchoolModel
            {
                object_id = school.object_id,
                school_name = school.school_name,
                address = school.address,
                main_hex_color = school.main_hex_color,
                secondary_hex_color = school.secondary_hex_color,
                school_tag = school.school_tag
            };
        }

        public SchoolModel(string school_name, string school_tag, string main_hex_color, string secondary_hex_color)
        {
            object_id = Guid.NewGuid();
            address = "TempAddress";
            this.school_name = school_name;
            this.school_tag = school_tag;
            this.main_hex_color = main_hex_color;
            this.secondary_hex_color = secondary_hex_color;
        }

        public void deleteEntity(School school)
        {
            //Creates new context and deletes local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.Schools.Remove(school);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }
        }


        public void updateEntity()
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                School school = (from o in context.Schools
                                 where o.object_id.Equals(object_id)
                                 select o).First();

                school.school_name = school_name;
                school.address = address;

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }
        }

        public void createEntity(School school)
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.Schools.Add(school);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }
        }

    }
}