using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class BusinessCategories
    {
        private string[] categories;
        private Dictionary<string, string[]> subCategories;

        public BusinessCategories() {
            // initialize the arrays of values
            categories = new string[]{ 
                "(choose a category)", 
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
                        "Auto Insurance",
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

        }

        public string[] Categories {
            get {
                return this.categories;
            }
        }

        public string[] Subcategories( string category ) {
            if( subCategories.ContainsKey( category ) ) {
                return subCategories[ category ];
            }
            else {
                return null;
            }
        }
    }
}
