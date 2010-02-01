using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Product : SimpleMrktSimItem
    {
        private ProductType productType;
        private bool isLeaf;

        public Product( string name, int id, ProductType productType, bool isLeaf )
            : base( name, id ) {
            this.productType = productType;
            this.isLeaf = isLeaf;
        }

        public ProductType ProductType {
            get {
                return productType;
            }
        }

        public bool IsLeaf {
            get {
                return isLeaf;
            }
        }

        public static Product All {
            get {
                return new Product( "All", -1, new ProductType( "All", -1 ), false );
            }
        }
    }

    public class ProductType : SimpleMrktSimItem
    {
        public ProductType( string name, int id )
            : base( name, id ) {
        }
    }

    public class Channel : SimpleMrktSimItem
    {
        public Channel( string name, int id )
            : base( name, id ) {
        }

        public static Channel All {
            get {
                return new Channel( "All", -1 );
            }
        }
    }

    public class Segment : SimpleMrktSimItem
    {
        public Segment( string name, int id )
            : base( name, id ) {
        }

        public static Segment All {
            get {
                return new Segment( "All", -1 );
            }
        }
    }

    public class SimpleMrktSimItem
    {
        private string  name;
        private int      id;

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// The internal MarketSim ID of the item
        /// </summary>
        public int ID {
            get { return id; }
        }

        /// <summary>
        /// Creates a new object.  For framework usage only.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public SimpleMrktSimItem( string name, int id ) {
            this.name = name;
            this.id = id;
        }
    }
}
