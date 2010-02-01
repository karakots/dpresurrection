using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaLibrary;
using System.IO;

namespace BusinessCategoryReader
{

    public partial class Form1 : Form
    {
        private DpMediaDb database;
        private string[] categories;
        private Dictionary<string, string[]> subCategories;
        private Dictionary<string, List<string>> subcategory_subtype;

        public Form1()
        {
            database = new DpMediaDb("Data Source=ENOCH-PC\\MSSMLBIZ;Initial Catalog=dpmedia;Integrated Security=True");
            database.RefreshWebData();
            InitializeComponent();

            #region categories
            categories = new string[]{ 
                "Automotive", 
                "Community & Education", 
                "Computer & Electronics", 
                "Finance",  
                "Food & Dining", 
                "Health & Medical",
                "Home Repair & Improvement", 
                "Legal", 
                "Personal Care", 
                "Real Estate", 
                "Shopping", 
                "Sports & Recreation", 
                "Travel",
                "Other"
            };

            subCategories = new Dictionary<string, string[]>();
            subCategories.Add( "Automotive",
                new string[]{    
                        "Auto Detailing", 
                        "Auto Emission Testing", 
                        "Auto Glass", 
                        "Auto Insurance", 
                        "Boating", 
                        "Car Dealers", 
                        "Car Loans", 
                        "Car Washing & Polishing", 
                        "Gas Stations", 
                        "Motorcycles", 
                        "Oil Change", 
                        "Parts", 
                        "Recreational Vehicles (RVs), Motor Homes", 
                        "Rental - Cars, RVs, Trucks", 
                        "Repair & Service", 
                        "Security", 
                        "Sound, Stereo", 
                        "Tire Dealers", 
                        "Towing Services", 
                        "Trucks", 
                        "Upholstery"
                    }
                );

            subCategories.Add( "Community & Education",
                new string[]{    
                       "Animal Shelters",
                       "Associations",
                       "Cemeteries",
                       "Food Banks",
                       "Government",
                       "Law",
                       "Libraries",
                       "Parks",
                       "Post Offices",
                       "Religion",
                       "Retirement Communities / Homes",
                       "Schools",
                       "Social Service & Welfare"
                    }
                );

            subCategories.Add( "Computer & Electronics",
                new string[]{    
                    "Cell Phones",
                    "Computer Networks",
                    "Computer Parts / Supplies",
                    "Computer Service & Repair",
                    "Computer Software",
                    "Computer Stores",
                    "Computer System Designers & Consultants",
                    "Computer Training",
                    "Consumer Electronics",
                    "Internet Service Providers",
                    "Internet Services" 
                    }
                );

            subCategories.Add( "Finance",
                new string[]{    
                        "Accountants",
                        "Banks",
                        "Bookkeepers",
                        "Certified Public Accountants (CPA)",
                        "Check Cashing",
                        "Credit Unions",
                        "Estate Planning",
                        "Financial Planners",
                        "Insurance",
                        "Insurance Advisors & Consultants",
                        "Mortgages and Real Estate Loans",
                        "Payday Loans",
                        "Personal Loans Personal Care",
                        "Stock & Bond Brokers"
                    }
                );

            subCategories.Add( "Legal",
                new string[]{    
                    "Appeals",
                    "Bankruptcy",
                    "Civil Rights",
                    "Corporate Business",
                    "Criminal Justice",
                    "Estate Planning",
                    "Family Law",
                    "Government",
                    "Insurance",
                    "Labor & Employment",
                    "Litigation",
                    "Medicare & Medicade",
                    "Patent, Trademark & Copyright Law",
                    "Personal Injury",
                    "Product Liability",
                    "Real Estate",
                    "Social Security",
                    "Tax"
                    }
                );

            
            subCategories.Add( "Food & Dining",
                new string[]{    
                    "Bagels",
                    "Bakers",
                    "Candy Stores",
                    "Caterers",
                    "Coffee Houses / Shops",
                    "Delicatessens",
                    "Donuts",
                    "Food Banks",
                    "Grocery Stores",
                    "Health & Diet Foods",
                    "Ice Cream Parlors",
                    "Liquor Stores",
                    "Pizza",
                    "Wineries"
                }
            );
            
            subCategories.Add( "Health & Medical",
                new string[]{    
                    "Acupuncture",
                    "Chiropractors",
                    "Clinics & Medical Centers",
                    "Dentists",
                    "Doctors",
                    "Health Clubs",
                    "Home Health Service",
                    "Hospitals",
                    "Massage Therapists",
                    "Medical Alarms",
                    "Nurses",
                    "Nursing & Convalescent Homes",
                    "Optometrists",
                    "Pharmacies / Drug Stores",
                    "Veterinarians",
                    "Vitamins & Food Supplements" 
                }
            );
            
            subCategories.Add( "Home Repair & Improvement",
                new string[]{    
                    "Air Conditioning Contractors & Systems",
                    "Architects",
                    "Bathroom Remodeling",
                    "Builders & Contractors",
                    " Carpet & Rug Cleaners",
                    "Carpet & Rug Dealers",
                    "Cement Contractors",
                    "Drywall Contractors",
                    "Electrical Contractors",
                    "Fence Contractors",
                    "Garden Centers",
                     "General Contractors",
                    "Hardware Stores",
                    "Home Builders",
                    "Home Improvement Stores",
                    "Interior Decorators",
                    "Kitchen Cabinets & Equipment",
                    "Landscape Contractors",
                    "Lumber Stores",
                    "Major Appliance Stores",
                    "Painters",
                    "Plumbing Contractors",
                    "Roofing Contractors",
                    "Security Systems",
                    "Septic Tanks",
                    "Shutters",
                    "Siding Contractors",
                    "Skylights & Solariums" 
                }
            );
            
            subCategories.Add( "Personal Care",
                new string[]{    
                    "Barbers",
                    "Beauty Salons",
                    "Cosmetics",
                    "Ear Piercing ",
                    "Electrolysis ",
                    "Hair Implants & Transplants",
                    "Health Clubs",
                    "Manicures and Pedicures",
                    "Massage",
                    "Massage Therapists",
                    "Spas",
                    "Tanning Salons",
                    "Tattoo Parlors",
                    "Tattoo Removal ",
                    "Wigs & Toupes" 
                }
            );
            
            subCategories.Add( "Real Estate",
                new string[]{    
                    "Apartments",
                    "Condominiums",
                    "Home Loans",
                    "Home Refinancing",
                    "Homes",
                    "Mortgage Bankers",
                    "Real Estate Appraisers",
                    "Real Estate Buyer Consultants",
                    "Time Shares"
                }
            );

            subCategories.Add( "Shopping",
                new string[]{    
                    "Apparel",
                    "Bakers",
                    "Book Stores",
                    "Bridal Shops",
                    "Carpets & Rugs",
                    "Convenience Stores",
                    "Department Stores",
                    "Florists",
                    "Furniture",
                    "Gift Shops",
                    "Jewelers",
                    "Pet Stores", 
                    "Pharmacies", 
                    "Photo Finishing", 
                    "Records, Tapes, & CDs",
                    "Shoe Stores",
                    "Shopping Malls", 
                    "Sporting Goods", 
                    "Video Tapes & DVD Rentals",
                    "Women's Apparel"
                }
            );

            subCategories.Add( "Sports & Recreation",
                new string[]{    
                    "Amusement Parks",
                    "Art Galleries",
                    "Bars / Nightclubs", 
                    "Bike Dealers", 
                    "Boating",
                    "Bowling Alleys", 
                    "Campgrounds",
                    "Dance & Gymnastics",
                    "Diving & Swimming",
                    "Fishing & Hunting",
                    "Golf Courses",
                    "Health Clubs & Gyms",
                    "Martial Arts",
                    "Museums",
                    "Parks & Playgrounds",
                    "Skating",
                    "Skiing",
                    "Sporting Goods",
                    "Tennis",
                    "Ticket Sales"
                }
            );

            subCategories.Add( "Travel",
                new string[]{    
                    "Airline Ticket Agencies",
                    "Airports",
                    "Bed & Breakfasts",
                    "Car Rentals",
                    "Hotels & Motels",
                    "Limousine Service",
                    "Luggage Stores",
                    "Taxis",
                    "Tourist Attractions",
                    "Travel Agencies & Bureaus"
                }
            );
            #endregion

        }

        private void GoBut_Click(object sender, EventArgs e)
        {
            parse_file();

            database.ClearCategoryData();
            database.Update();
            database.RefreshWebData();

            foreach (string category in subCategories.Keys)
            {
                database.AddCategory(category);
            }

            database.Update();
            database.RefreshWebData();

            foreach (string category in subCategories.Keys)
            {
                foreach (string subcategory in subCategories[category])
                {
                    database.AddSubCategory(subcategory, category);
                }
            }

            database.Update();
            database.RefreshWebData();

            foreach (string subcat in subcategory_subtype.Keys)
            {
                foreach (string subtype in subcategory_subtype[subcat])
                {
                    database.AddCategorySubtype(subtype, subcat);
                }
            }

            database.Update();
            database.RefreshWebData();

            MessageBox.Show("All Finished");
        }

        private void parse_file()
        {
            StreamReader reader = new StreamReader(FileName.Text);

            string first_line = reader.ReadLine();
            string[] split_subs = first_line.Split('\t');
            List<string> subtypes = new List<string>();
            foreach (string raw_sub in split_subs)
            {
                if(raw_sub.Length > 0)
                {
                    subtypes.Add(format(raw_sub));
                }
            }

            subcategory_subtype = new Dictionary<string,List<string>>();

            while (!reader.EndOfStream)
            {
                string[] line = reader.ReadLine().Split('\t');
                string subcat = line[0];
                if(!subcategory_subtype.ContainsKey(subcat))
                {
                    subcategory_subtype.Add(subcat, new List<string>());
                }
                for (int i = 1; i < line.Length; i++)
                {
                    if (line[i].Trim() == "1")
                    {
                        subcategory_subtype[subcat].Add(subtypes[i - 1]);
                    }
                }
            }
        }

        private string format(string name)
        {
            return name.ToUpper().Replace(" ", "");
        }

        private void BrowseBut03_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileName.Text = dialog.FileName;
        }
            


    }
}
