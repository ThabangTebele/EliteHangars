﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EliteHangers
{
    public partial class dashboard3 : System.Web.UI.Page
    {
        string query;
        SQL sql = new SQL();
        int selectedStartDate = 0;



        //Booking Variables
        bool SC1;
        bool SC2;
        DateTime dtStart;
        DateTime dtEnd;
        DateTime dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                query = "SELECT DISTINCT name, city_id FROM City";
                sql.comboBox(query, "City", "name", DropDownList1, "city_id");

                query = $"SELECT * FROM Hangar ";
                sql.comboBox(query, "Hangar", "name", DropDownList2, "hangar_id");
                //CalendarStart.SelectedDate = DateTime.Today;
                DropDownList2.SelectedIndex = -1;
                Session["hangarID"] = null;
                Session["startDate"] = null;
                Session["endDate"] = null;
                Session["customerId"] = null;
            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Session["sc1"] = null;
            Session["sc2"] = null;
            Session["hangarID"] = null;
            Session["sc2Date"] = null;
            if(DropDownList1.Text!="")
            {
                query = $" SELECT  Hangar.name, Hangar.hangar_id FROM Hangar INNER JOIN City ON Hangar.city_id = City.city_id WHERE City.name = '{DropDownList1.SelectedItem}'";
                sql.comboBox(query, "Hangar", "name", DropDownList2, "hangar_id");
            }
            else
            {
                CalendarStart.SelectedDate = DateTime.MinValue;
                CalendarEnd.SelectedDate = DateTime.MinValue;
                DropDownList2.Items.Clear();
            }
            
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            //insert it into database
            //check if hangarID!=null;
            //check if sc1 is not null
            //check if sc2 is not null
            //then insert into db
        }

        protected void CalendarStart_SelectionChanged(object sender, EventArgs e)
        {
            Session["sc2"] = true;
            Session["sc1Date"] = CalendarStart.SelectedDate;
        }

        protected void CalendarStart_DayRender(object sender, DayRenderEventArgs e)
        {
            
            if (Session["sc1"] != null && Session["hangarID"]!=null)
            {
                //exclude past dates and booked days
                query = $"select date_start, date_end from Booking where hangar_id = {Session["hangarID"].ToString()}; ";
                List<DateTime> dateList = sql.databaseDates(query);
                for (int i = 0; i < dateList.Count; i = i + 2)
                {
                    if (dateList[i] <= e.Day.Date && dateList[i + 1] >= e.Day.Date)
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                if (e.Day.Date < DateTime.Today)
                {
                    e.Day.IsSelectable = false;
                    e.Cell.ForeColor = System.Drawing.Color.Gray;
                }
            }
            else
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void CalendarEnd_DayRender(object sender, DayRenderEventArgs e)
        {

            if(Session["sc2"] != null)
            {
                //show them dates from start date to day before next booked day
                query = $"select min(date_start) from Booking where date_start > '{DateTime.Parse(Session["sc1Date"].ToString())}' AND hangar_id = {Session["hangarID"].ToString()}; ";
                string nextBooking = sql.nextBooking(query);
                dtStart = DateTime.Parse(Session["sc1Date"].ToString());
                if (nextBooking != "")
                {
                    dt = DateTime.Parse(nextBooking);
                    
                    if (dtStart <= e.Day.Date && e.Day.Date <dt)
                    {
                        e.Day.IsSelectable = true;
                    }
                    else
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                else
                {
                    if(!(dtStart <= e.Day.Date))
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                    
                }
                
            }
            else
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.DarkGray;
            }
        }

        protected void CalendarEnd_SelectionChanged(object sender, EventArgs e)
        {
            Session["sc2Date"] = CalendarEnd.SelectedDate;
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.Text != "")
            {
                //use has slected hanagar now allow them to 
                Session["sc1"] = true;
                Session["sc2"] = null;
                Session["hangarID"] = DropDownList2.SelectedValue; 
            }
            else
            {
                Session["sc1"] = null;
                Session["sc2"] = null;
                CalendarStart.SelectedDate = DateTime.MinValue;
                CalendarEnd.SelectedDate = DateTime.MinValue;
            }
        }
    }
}