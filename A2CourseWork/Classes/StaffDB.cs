﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A2CourseWork.Objects;
using System.Data.SqlClient;
namespace A2CourseWork.Classes
{
    class StaffDB
    {
        Database db;
        public StaffDB(Database db)
        {
            this.db = db;
        }

        public List<StaffMember> getallstaff()
        {
            List<StaffMember> results = new List<StaffMember>();
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = "SELECT * FROM Staff";
            db.Rdr = db.Cmd.ExecuteReader();
            while (db.Rdr.Read())
            {
                results.Add(getstaffFromReader(db.Rdr));
            }
            db.Rdr.Close();
            return results;
        }

        public StaffMember getstaffFromReader(SqlDataReader reader)
        {
            StaffMember member = new StaffMember();
            member.StaffId = reader.GetInt32(0);
            member.Forename = reader.GetString(1);
            member.Surname = reader.GetString(2);
            member.TeleNo = reader.GetString(3);
            member.Address = reader.GetString(4);
            member.Postcode = reader.GetString(5);
            return member;
        }

        public void addstaffmember(string forename,string surname,string teleno,string postcode,string address)
        {
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = "INSERT INTO Staff(Forename,Surname,TeleNo,Address,Postcode) VALUES('" + forename+ "','" + surname + "', '" + teleno + "', '" + address + "', '" + postcode + "')";
            db.Cmd.ExecuteNonQuery();
        }

        public void updatestaffmember(string oldForename, string forename, string surname, string teleno, string postcode, string address)
        {
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = "UPDATE Staff SET Forename = '" + forename + "',Surname = '" + surname + "',TeleNo = '" + teleno + "', Postcode = '" + postcode + "',Address = '" + address + "' WHERE Forename = '" + oldForename +"'";
            db.Cmd.ExecuteNonQuery();
        }

        public void updateStaffGroup(string forename,int groupid)
        {
            int id = getStaffID(forename);
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"UPDATE GroupRota SET GroupID={groupid} WHERE StaffID ={id}";
            db.Cmd.ExecuteNonQuery();
        }

        public void removeStaffmember(string forename)
        {
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"DELETE GroupRota FROM GroupRota INNER JOIN Staff ON GroupRota.StaffID = Staff.StaffId WHERE Forename = '{forename}'";
            db.Cmd.ExecuteNonQuery();

            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"DELETE FROM Staff WHERE Forename = '{forename}'";
            db.Cmd.ExecuteNonQuery();
        }

        public void addStaffGroup(string forename,int groupid)
        {
            int id = getStaffID(forename);
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"INSERT INTO GroupRota(StaffID,GroupID) VALUES({id},{groupid})";
            db.Cmd.ExecuteNonQuery();
        }

        public int DeleteDates(string Forename)
        {
            int groupid = getStaffGroup(Forename);
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"UPDATE Dates Set Active = 0 From Dates INNER JOIN Booking ON Dates.BookingId = Booking.BookingID WHERE Booking.GroupId = {groupid}";
            int num = db.Cmd.ExecuteNonQuery();
            return num;
        }

        public int UpdateDates(int groupid)
        {
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"UPDATE Dates Set Active = 1 From Dates INNER JOIN Booking ON Dates.BookingId = Booking.BookingID WHERE Booking.GroupId = {groupid}";
            int num = db.Cmd.ExecuteNonQuery();
            return num;
        }

        public int countStaff(string Forename)
        {
            int groupid = getStaffGroup(Forename);
            int id = 0;
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"SELECT COUNT(*) FROM GroupRota WHERE GroupID = {groupid}";
            db.Rdr = db.Cmd.ExecuteReader();
            while (db.Rdr.Read())
            {
                id = db.Rdr.GetInt32(0);
            }
            db.Rdr.Close();
            return id;
        }

        public int countStaffbyid(int groupid)
        {
            int id = 0;
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"SELECT COUNT(*) FROM GroupRota WHERE GroupID = {groupid}";
            db.Rdr = db.Cmd.ExecuteReader();
            while (db.Rdr.Read())
            {
                id = db.Rdr.GetInt32(0);
            }
            db.Rdr.Close();
            return id;
        }

        private int getStaffID(string forename)
        {
            int id = 0;
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"SELECT StaffId FROM Staff WHERE ForeName = '{forename}'";
            db.Rdr = db.Cmd.ExecuteReader();
            while (db.Rdr.Read())
            {
                id = db.Rdr.GetInt32(0);
            }
            db.Rdr.Close();
            return id;
        }

        public List<string> getallgroups()
        {
            List<string> results = new List<string>();
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = "SELECT GroupName FROM Groups";
            db.Rdr = db.Cmd.ExecuteReader();
            if (db.Rdr.Read())
            {
                results.Add(db.Rdr.GetString(0));
            }
            db.Rdr.Close();
            return results;
        }

        public int getStaffGroup(string Forename)
        {
            int results = 0;
            db.Cmd = db.Conn.CreateCommand();
            db.Cmd.CommandText = $"SELECT GroupID FROM GroupRota INNER JOIN Staff ON GroupRota.StaffID = Staff.StaffId WHERE Staff.ForeName = '{Forename}'";
            db.Rdr = db.Cmd.ExecuteReader();
            if (db.Rdr.Read())
            {
                results = db.Rdr.GetInt32(0);
            }
            db.Rdr.Close();
            return results;
        }
    }
}
