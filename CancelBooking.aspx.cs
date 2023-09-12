﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EliteHangers
{
    public partial class CancelBooking : System.Web.UI.Page
    {
        private string query;
        private SQL sql = new SQL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["user"] == null)
                {
                    UserAuth user = (UserAuth)Session["user"];
                    if (user.role == 2)//customer
                    {
                        if (!IsPostBack)
                        {
                            query = "SELECT booking_id FROM Booking where customer_id = " + user.id;
                            sql.comboBox(query, "Booking", "booking_id", ddlBookingID, "booking_id");
                        }
                    }
                    else if (user.role == 1)
                    {
                        //manager

                    }
                    else
                    {
                        //0
                        //employee clerk
                    }

                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch(Exception)
            {
                //insert error message.
            }
           
        }


        protected void ddlBookingID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //get the booking id
                //set booking to 2

            }
            catch(Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}