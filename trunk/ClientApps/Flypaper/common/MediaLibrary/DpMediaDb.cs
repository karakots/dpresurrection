using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using DemographicLibrary;
using GeoLibrary;
using Utilities;

namespace MediaLibrary
{

    /// <summary>
    /// Class for reading and writing media data to database
    /// </summary>
    public class DpMediaDb
    {
        Media db;

        private string connectionString { get; set; }

        private BinaryFormatter serializer;
        private MediaTableAdapters.TableAdapterManager adaptMan;
        private Dictionary<int, Dictionary<int, AdOption>> type_options;


        public DpMediaDb(string connStr)
        {
            connectionString = connStr;

            serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            db = new Media();          
           
            adaptMan = new MediaLibrary.MediaTableAdapters.TableAdapterManager();

            adaptMan.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.mediaTableAdapter = new MediaLibrary.MediaTableAdapters.mediaTableAdapter();

            adaptMan.mediaTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.ad_optionsTableAdapter = new MediaLibrary.MediaTableAdapters.ad_optionsTableAdapter();

            adaptMan.ad_optionsTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );



            adaptMan.media_subtypeTableAdapter = new MediaLibrary.MediaTableAdapters.media_subtypeTableAdapter();

            adaptMan.media_subtypeTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );



            adaptMan.regionTableAdapter = new MediaLibrary.MediaTableAdapters.regionTableAdapter();

            adaptMan.regionTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );



            adaptMan.vehicleTableAdapter = new MediaLibrary.MediaTableAdapters.vehicleTableAdapter();

            adaptMan.vehicleTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.anti_vcl_optionTableAdapter = new MediaLibrary.MediaTableAdapters.anti_vcl_optionTableAdapter();

            adaptMan.anti_vcl_optionTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );



            adaptMan.region_treeTableAdapter = new MediaLibrary.MediaTableAdapters.region_treeTableAdapter();

            adaptMan.region_treeTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );



            adaptMan.media_demographicTableAdapter = new MediaLibrary.MediaTableAdapters.media_demographicTableAdapter();

            adaptMan.media_demographicTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.business_categoryTableAdapter = new MediaLibrary.MediaTableAdapters.business_categoryTableAdapter();

            adaptMan.business_categoryTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.business_subcategoryTableAdapter = new MediaLibrary.MediaTableAdapters.business_subcategoryTableAdapter();

            adaptMan.business_subcategoryTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            adaptMan.category_subtypeTableAdapter = new MediaLibrary.MediaTableAdapters.category_subtypeTableAdapter();

            adaptMan.category_subtypeTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );

            adaptMan.vehicle_regionTableAdapter = new MediaLibrary.MediaTableAdapters.vehicle_regionTableAdapter();

            adaptMan.vehicle_regionTableAdapter.Connection = new System.Data.SqlClient.SqlConnection( connectionString );


            type_options = null;

        }

        #region Media Manager Updates
        /// <summary>
        /// Clear database
        /// </summary>
        public void DeleteFromDatabase()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            command.Connection = new System.Data.SqlClient.SqlConnection( adaptMan.Connection.ConnectionString );

       
            command.Connection.Open();
            command.CommandText = "delete from region_tree";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from media_demographic";
            command.ExecuteNonQuery();
            command.Connection.Close();


            command.Connection.Open();
            command.CommandText = "delete from vehicle_options";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from vehicle";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from region";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from media_subtype";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from ad_options";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from media";
            command.ExecuteNonQuery();
            command.Connection.Close();
 
            db.Clear();
        }

        //public void DeleteFromVehicleDatabase(MediaVehicle.MediaType type)
        //{
        //    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
        //    command.CommandTimeout = 0;

        //    command.Connection = new System.Data.SqlClient.SqlConnection( adaptMan.Connection.ConnectionString );

        //    command.CommandText = "DELETE FROM vehicle " +
        //        " WHERE vehicle.subtype IN  " + 
        //        " (SELECT  media_subtype.id  FROM media_subtype, media " + 
        //        " WHERE media_subtype.media = media.id AND media.name = '" + type.ToString() + "')";

        //    command.Connection.Open();
        //    command.ExecuteNonQuery();

        //    command.CommandText = "DELETE FROM ad_options " +
        //        " WHERE ad_options.type_id IN  " +
        //        " (SELECT  media.id  FROM media " +
        //        " WHERE media.name = '" + type.ToString() + "')";

        //    command.Connection.Open();
        //    command.ExecuteNonQuery();
        //    command.Connection.Close();

        //    db.vehicle.Clear();
        //}

        public void DeleteFromDemographicDatabase( MediaVehicle.MediaType type )
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            command.CommandTimeout = 0;

            command.Connection = new System.Data.SqlClient.SqlConnection( adaptMan.Connection.ConnectionString );

            command.CommandText = "DELETE FROM media_demographic " +
                " WHERE media_demographic.subtype IN  " +
                " (SELECT  media_subtype.id  FROM media_subtype, media " +
                " WHERE media_subtype.media = media.id AND media.name = '" + type.ToString() + "')";

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            db.media_demographic.Clear();
        }

        public void Clear()
        {
            db.Clear();
            db.AcceptChanges();
        }


        public void ClearMedia()
        {
            db.anti_vcl_option.Clear();
            db.vehicle.Clear();
            db.media_demographic.Clear(); // make sure this is clear as well
            db.media_subtype.Clear();
            db.ad_options.Clear();
            db.media.Clear();

            db.AcceptChanges();
        }

        public void ClearVehicle()
        {
            db.anti_vcl_option.Clear();
            db.vehicle.Clear();

            db.AcceptChanges();
        }

        // test algorithm

        public bool Test()
        {
            Demographic.PrimVector orig = new Demographic.PrimVector();

            orig.Any = -1;
            for( int ii = 0; ii < orig.Count; ++ii )
            {
                orig[ii] = ii;
            }

            // compress
            byte[] compress = primVecToBytes( orig );

            // decompress
            Demographic.PrimVector vec = bytesToPrimVec( compress );

            // compare
            bool match = true;


            if( vec.Any != orig.Any )
            {
                match = false;
            }

            if( vec.Count != orig.Count )
            {
                match = false;
            }

            if( match )
            {
                for( int ii = 0; ii < orig.Count; ++ii )
                {
                    if( vec[ii] != orig[ii] )
                    {
                        match = false;
                    }
                }
            }

            return match;
        }

        public void RefreshMedia()
        {
            db.Clear();

            adaptMan.mediaTableAdapter.Fill( db.media );
            adaptMan.ad_optionsTableAdapter.Fill(db.ad_options);
            adaptMan.media_subtypeTableAdapter.Fill( db.media_subtype );
            adaptMan.regionTableAdapter.Fill( db.region );
            adaptMan.vehicleTableAdapter.Fill( db.vehicle );
            adaptMan.vehicle_regionTableAdapter.Fill( db.vehicle_region );
            adaptMan.anti_vcl_optionTableAdapter.Fill( db.anti_vcl_option );
        }

        // temp method to move info to table
        //
        //public void ConvertVehicles()
        //{
        //    RefreshMedia();


        //    foreach( Media.vehicleRow row in db.vehicle )
        //    {
        //        MemoryStream str = new MemoryStream( row.vehicle );

        //        MediaVehicle vehicle = (MediaVehicle)serializer.Deserialize( str );

        //        row.cpm = vehicle.CPM;
        //        row.cycle = (byte) vehicle.Cycle;
        //        row.info = vehicle.URL;
                
        //        // special cases
        //        Newspaper news = vehicle as Newspaper;

        //        if( news != null )
        //        {
        //            if( news.Distribute == Newspaper.Distribution.Sunday )
        //            {
        //                row.cycle = (byte)MediaVehicle.AdCycle.Weekly;
        //            }
        //            else
        //            {
        //                row.cycle = (byte)MediaVehicle.AdCycle.Daily;
        //            }
        //        }
        //        else
        //        {

        //            Magazine mag = vehicle as Magazine;

        //            if( mag != null )
        //            {
        //                string nat = "";
        //                if (mag.DMAAds)
        //                {
        //                    nat = "N";
        //                }

        //                if( mag.NationalAds )
        //                {
        //                    if( nat != "" )
        //                    {
        //                        nat += ",";
        //                    }

        //                    nat += "D";
        //                }

        //                row.geo_info = nat;
        //            }
        //            else
        //            {
        //                Yellowpages yp = vehicle as Yellowpages;

        //                if( yp != null )
        //                {
        //                    row.geo_info = yp.City;
        //                }
        //            }
        //        }
        //    }


        //    Update();
        //}


        public void RefreshAllMediaData()
        {
            db.Clear();

            adaptMan.mediaTableAdapter.Fill( db.media );
            adaptMan.ad_optionsTableAdapter.Fill(db.ad_options);
            adaptMan.media_subtypeTableAdapter.Fill( db.media_subtype );
            adaptMan.regionTableAdapter.Fill( db.region );
            adaptMan.vehicleTableAdapter.Fill( db.vehicle );
            adaptMan.vehicle_regionTableAdapter.Fill( db.vehicle_region );
            adaptMan.anti_vcl_optionTableAdapter.Fill( db.anti_vcl_option );
            adaptMan.media_demographicTableAdapter.Fill( db.media_demographic );
        }


        public void RefreshAllMediaData(GeoRegion reg)
        {
            db.Clear();

            adaptMan.mediaTableAdapter.Fill( db.media );
            adaptMan.ad_optionsTableAdapter.Fill( db.ad_options );
            adaptMan.media_subtypeTableAdapter.Fill( db.media_subtype );
            adaptMan.regionTableAdapter.Fill( db.region );

            if( reg == null )
            {
                adaptMan.vehicleTableAdapter.Fill( db.vehicle );

                adaptMan.vehicle_regionTableAdapter.Fill( db.vehicle_region );
                adaptMan.anti_vcl_optionTableAdapter.Fill( db.anti_vcl_option );
            }
            else
            {
                // get id of region
                Media.regionRow regRow = db.region.First( row => row.name == reg.Name );

                foreach( Media.mediaRow media in db.media )
                {
                    adaptMan.vehicleTableAdapter.FillFromRegion( db.vehicle, regRow.id, media.id );
                }
                // TBD - need query to read in only those items in table for this region
                // adaptMan.vehicle_regionTableAdapter.Fill( db.vehicle_region );
                adaptMan.anti_vcl_optionTableAdapter.FillBy(db.anti_vcl_option, reg.Name );
            }

          
            adaptMan.media_demographicTableAdapter.Fill( db.media_demographic );
        }

        public void RefreshRegionInfo()
        {
            db.Clear();

            adaptMan.regionTableAdapter.Fill( db.region );
            adaptMan.region_treeTableAdapter.Fill( db.region_tree );
        }

        private void computeSubTree( Media.regionRow parentRow, GeoRegion parentReg )
        {
            // find all sub regions of parent
            if( parentRow.Getregion_treeRowsByFK_region_tree_parent_region().Length > 0 )
            {
                foreach( Media.region_treeRow treeRow in parentRow.Getregion_treeRowsByFK_region_tree_parent_region() )
                {
                    Media.regionRow regRow = treeRow.regionRowByFK_region_tree_child_region;
                    GeoRegion.RegionType type = (GeoRegion.RegionType)regRow.type;

                    GeoRegion geoReg = new GeoRegion( regRow.name, (GeoRegion.RegionType)regRow.type );
                    

                    geoReg.Box.Min.x = regRow.min_x;
                    geoReg.Box.Min.y = regRow.min_y;
                    geoReg.Box.Max.x = regRow.max_x;
                    geoReg.Box.Max.y = regRow.max_y;
                    geoReg.Id = regRow.id;

                    parentReg.Add( geoReg );


                    computeSubTree( regRow, geoReg );
                }
            }
            else
            {
                // check if children in db not read in
                MediaLibrary.MediaTableAdapters.regionTableAdapter regAdpater = new MediaLibrary.MediaTableAdapters.regionTableAdapter();
                regAdpater.Connection = new System.Data.SqlClient.SqlConnection( connectionString );
                using( Media.regionDataTable tbl = regAdpater.GetRegionByParent( parentRow.id ) )
                {

                    foreach( Media.regionRow regRow in tbl )
                    {
                        GeoInfo city = new GeoInfo( regRow.geo_id );

                        city.Name = regRow.name;
                        city.Lat = regRow.min_y;
                        city.Long = regRow.min_x;
                        city.Id = regRow.id;

                        parentReg.Add( city );
                    }
                }
            }
        }

        // mimics GeoRegion read
        public GeoRegion ComputeRegionTree()
        {
            GeoRegion topGeo = null;

            // first find top geo object
            if( db.region.Any( row => row.Getregion_treeRowsByFK_region_tree_child_region().Length == 0 ) )
            {
                Media.regionRow topGeoRow = db.region.First( row => row.Getregion_treeRowsByFK_region_tree_child_region().Length == 0 );

                topGeo = new GeoRegion( topGeoRow.name, (GeoRegion.RegionType) topGeoRow.type );

                topGeo.Box.Min.x = topGeoRow.min_x;
                topGeo.Box.Min.y = topGeoRow.min_x;
                topGeo.Box.Max.x = topGeoRow.max_x;
                topGeo.Box.Max.y = topGeoRow.max_y;
                topGeo.Id = topGeoRow.id;
               

                computeSubTree( topGeoRow, topGeo );

            }

            return topGeo;
        }

        private void updateRegionTree( int parent, int child )
        {

            if( db.region_tree.Any( row => row.parent == parent && row.child == child ) )
            {
                return;
            }

            Media.region_treeRow rgt = db.region_tree.Newregion_treeRow();

            rgt.parent = parent;
            rgt.child = child;

            db.region_tree.Addregion_treeRow( rgt );
        }

        private Media.regionRow updateRegion( Media.regionRow parentRow, GeoRegion reg )
        {
            // check if region exists
            Media.regionRow dbReg = GetRegion(parentRow, reg );

            if( dbReg == null )
            {
                // if not create
                dbReg = db.region.NewregionRow();


                dbReg.demo = new byte[0];
                dbReg.name = reg.Name;
                dbReg.max_x = reg.Box.Max.x;
                dbReg.max_y = reg.Box.Max.y;
                dbReg.min_x = reg.Box.Min.x;
                dbReg.min_y = reg.Box.Min.y;
                dbReg.type = (byte)reg.Type;

                GeoInfo city = reg as GeoInfo;

                if( city != null )
                {
                    dbReg.geo_id = city.GeoID;
                }
                else
                {
                    dbReg.geo_id = -1;
                }


                db.region.AddregionRow( dbReg );
            }
            else
            {
                dbReg.name = reg.Name;
                dbReg.max_x = reg.Box.Max.x;
                dbReg.max_y = reg.Box.Max.y;
                dbReg.min_x = reg.Box.Min.x;
                dbReg.min_y = reg.Box.Min.y;
                dbReg.type = (byte)reg.Type;

                GeoInfo city = reg as GeoInfo;

                if( city != null )
                {
                    dbReg.geo_id = city.GeoID;
                }
                else
                {
                    dbReg.geo_id = -1;
                }
            }

           
            return dbReg;
        }

        public int AddRegion( Media.regionRow parentRow, GeoRegion region )
        {

            Media.regionRow rg = updateRegion( parentRow, region );

            int regionId = rg.id;

            if( region.SubRegions != null )
            {

                foreach( GeoRegion subregion in region.SubRegions )
                {
                    int subId = AddRegion( rg, subregion );

                    updateRegionTree( regionId, subId );
                }
            }

            return regionId;
        }

        public Media.regionRow GetRegion(Media.regionRow parentRow, GeoRegion reg )
        {
            if( parentRow != null )
            {
                if( parentRow.Getregion_treeRowsByFK_region_tree_parent_region().Any( row => row.regionRowByFK_region_tree_child_region.name == reg.Name ) )
                {
                    return parentRow.Getregion_treeRowsByFK_region_tree_parent_region().First(
                        row => row.regionRowByFK_region_tree_child_region.name == reg.Name ).regionRowByFK_region_tree_child_region;
                }
            }

            // find region that matches this name

            if( db.region.Any( row => row.name == reg.Name && row.Getregion_treeRowsByFK_region_tree_child_region().Length == 0 ) )
            {
                return db.region.First( row => row.name == reg.Name && row.Getregion_treeRowsByFK_region_tree_child_region().Length == 0 );
            }

            //int count = db.region.Count( row => row.name == reg.Name 
            //    && row.Getregion_treeRowsByFK_region_tree_child_region().Any(parRow => parRow == parentRow.id) );

            //if( count == 1 )
            //{
            //    rval = db.region.First( row => row.name == reg.Name );
            //}

            return null;
        }


        public void RefreshWebData()
        {
            db.Clear();

            adaptMan.mediaTableAdapter.Fill( db.media );
            adaptMan.ad_optionsTableAdapter.Fill(db.ad_options);
            adaptMan.media_subtypeTableAdapter.Fill( db.media_subtype );
           
            adaptMan.regionTableAdapter.FillRegionsOnly( db.region );
            adaptMan.region_treeTableAdapter.FillOnlyRegions( db.region_tree );

            adaptMan.anti_vcl_optionTableAdapter.Fill( db.anti_vcl_option );
            //adaptMan.vehicleTableAdapter.Fill( db.vehicle );
            //adaptMan.vehicle_regionTableAdapter.Fill( db.vehicle_region );
          
            adaptMan.business_categoryTableAdapter.Fill(db.business_category);
            adaptMan.business_subcategoryTableAdapter.Fill(db.business_subcategory);
            adaptMan.category_subtypeTableAdapter.Fill(db.category_subtype);
        }

        public Dictionary<string, Demographic.PrimVector> RefreshRegionData()
        {
            Dictionary<string, Demographic.PrimVector> rval = new Dictionary<string, Demographic.PrimVector>();
            db.Clear();
            adaptMan.regionTableAdapter.Fill( db.region );

            foreach( Media.regionRow reg in db.region )
            {
                Demographic.PrimVector vec = this.bytesToPrimVec( reg.demo );

                // normalize vector
                vec.Normalize();

                rval.Add( reg.name, vec );
            }

            return rval;
        }

        /// <summary>
        /// write data to database
        /// </summary>
        public void Update()
        {
            //adaptMan.mediaTableAdapter.Update( db.media );
            //adaptMan.media_subtypeTableAdapter.Update( db.media_subtype );
            //adaptMan.regionTableAdapter.Update( db.region );
            //adaptMan.vehicleTableAdapter.Update( db.vehicle );
            //adaptMan.region_treeTableAdapter.Update( db.region_tree );
            //adaptMan.demographicTableAdapter.Update( db.demographic );

            if( db.HasChanges() )
            {
               int numChanges = adaptMan.UpdateAll( db );

               if( numChanges == 0 )
               {
                   return;
               }
            }
        }

        public List<MediaVehicle> ReadAllFromDb()
        {
            // fill tables
            RefreshAllMediaData();

            List<MediaVehicle> rval = new List<MediaVehicle>();

            foreach( Media.vehicleRow vcl in db.vehicle )
            {
                int type_id = db.media_subtype.FindByid( vcl.subtype ).media;
                string subtypeName = db.media_subtype.FindByid( vcl.subtype ).name;
                string regionName = null;
                double size = 0;
                foreach(Media.vehicle_regionRow vclReg in vcl.Getvehicle_regionRows())
                {
                    if (vclReg.size > size)
                    {
                        regionName = vclReg.regionRow.name;
                        size = vclReg.size;
                    }
                }

                size *= DpMediaDb.US_HH_SIZE;

                MediaVehicle media = new MediaVehicle( vcl, GetTypeForID( type_id ), subtypeName, regionName, size, GetTypeOptions( type_id ) );

                IEnumerable<Media.anti_vcl_optionRow> exclude = excludedOptions( vcl.id );
                if( exclude != null )
                {
                    media.ExcludeOptions( exclude );
                }

                // update 
                rval.Add( media );
            }

            // RefreshVehicleOptions(rval, true);

            return rval;
        }

        // just read a simple list of vehicles
        public List<MediaVehicle> ReadMediaFromDb()
        {
            // fill tables
            RefreshMedia();

            List<MediaVehicle> rval = new List<MediaVehicle>();

            foreach( Media.vehicleRow row in db.vehicle )
            {
                //MemoryStream str = new MemoryStream( row.vehicle );

                //MediaVehicle vehicle = (MediaVehicle)serializer.Deserialize( str );
               
                //rval.Add( vehicle );
            }

            // RefreshVehicleOptions(rval, true);

            return rval;
        }

        public void RefreshAdOptions()
        {
            db.Clear();

            adaptMan.mediaTableAdapter.Fill( db.media );
            adaptMan.ad_optionsTableAdapter.Fill( db.ad_options );
        }

        public void UpdateAdOption( AdOption option )
        {

            if( db.ad_options.Any( row => row.id == option.ID ) )
            {
                Media.ad_optionsRow adRow = db.ad_options.First( row => row.id == option.ID );

                MemoryStream str = new MemoryStream();

                serializer.Serialize( str, option );

                adRow.ad_option = str.GetBuffer();
                adRow.name = option.Name;
            }
            else
            {

            }
        }


        public List<T> ReadOptions<T>() where T : class, AdOption
        {
            List<T> rval = new List<T>();

            foreach( Media.ad_optionsRow adRow in db.ad_options )
            {
                MemoryStream str = new MemoryStream( adRow.ad_option );
                AdOption option = (AdOption)serializer.Deserialize( str );


                if( option.GetType() == typeof( T ) )
                {
                    option.MediaType = adRow.mediaRow.name;

                    rval.Add( option as T );
                }
            }

            return rval;
        }

        public Dictionary<int,AdOption> ReadOptions()
        {
            Dictionary<int, AdOption> rval = new Dictionary<int, AdOption>();

            foreach( Media.ad_optionsRow adRow in db.ad_options )
            {
                MemoryStream str = new MemoryStream( adRow.ad_option );
                AdOption option = (AdOption)serializer.Deserialize( str );

                rval.Add( adRow.id, option );
            }

            return rval;
        }


        //private void RefreshVehicleOptions(IEnumerable<MediaVehicle> vehicles, bool force_refresh)
        //{
        //    if (ad_options == null || force_refresh)
        //    {
        //        ad_options = new Dictionary<int, AdOption>();

        //        foreach (Media.ad_optionsRow row in db.ad_options)
        //        {
        //            MemoryStream str = new MemoryStream(row.ad_option);
        //            AdOption option = (AdOption)serializer.Deserialize(str);
        //            ad_options.Add(row.id, option);
        //        }
        //    }

        //    if (vehicle_options == null || force_refresh)
        //    {
        //        vehicle_options = new Dictionary<Guid, List<int>>();
        //    }

        //    foreach (MediaVehicle vehicle in vehicles)
        //    {
        //        if (!vehicle_options.ContainsKey(vehicle.Guid))
        //        {
        //            GetMediaVehicleOptions(vehicle.Guid);
        //        }

        //        foreach (int option_id in vehicle_options[vehicle.Guid])
        //        {
        //            if (!ad_options.ContainsKey(option_id))
        //            {
        //                continue;
        //            }
        //            vehicle.AddOption(option_id, ad_options[option_id]);
        //        }
        //    }
        //}

        //private void RefreshVehicleOptions(MediaVehicle vehicle)
        //{
        //    if (ad_options == null)
        //    {
        //        ad_options = new Dictionary<int, AdOption>();
        //        foreach (Media.ad_optionsRow row in db.ad_options)
        //        {
        //            MemoryStream str = new MemoryStream(row.ad_option);
        //            AdOption option = (AdOption)serializer.Deserialize(str);
        //            ad_options.Add(row.id, option);
        //        }
        //    }

        //    if (vehicle_options == null)
        //    {
        //        vehicle_options = new Dictionary<Guid, List<int>>();
        //    }

        //    if (!vehicle_options.ContainsKey(vehicle.Guid))
        //    {
        //        GetMediaVehicleOptions(vehicle.Guid);
        //    }


        //    foreach (int option_id in vehicle_options[vehicle.Guid])
        //    {
        //        if (!ad_options.ContainsKey(option_id))
        //        {
        //            continue;
        //        }
        //        vehicle.AddOption(option_id, ad_options[option_id]);
        //    }
        //}

        //public void WriteVehiclesToDb( Dictionary<Media.vehicleRow, MediaVehicle> vcls )
        //{
        //    int index = 0;

        //    foreach( Media.vehicleRow row in db.vehicle )
        //    {
        //        string media = vcls[row].Type.ToString();

        //        // set options to null as we ar enot serializing these
        //        Dictionary<int, AdOption> tmp = vcls[row].GetOptions();

        //        vcls[row].SetOptions( null );

        //        MemoryStream str = new MemoryStream();

                
        //        serializer.Serialize( str, vcls[row] );

        //        row.vehicle = str.GetBuffer();

        //        str.Close();

        //        // restore options
        //        vcls[row].SetOptions( tmp );

        //        // update region if it has changed
        //        if( vcls[row].RegionName != row.regionRow.name )
        //        {
        //            Media.regionRow regRow = null;
        //            try
        //            {
        //                regRow = db.region.First( reg => reg.name == vcls[row].RegionName );
        //            }
        //            catch( Exception e )
        //            {
        //            }

        //            if( regRow != null )
        //            {
        //                row.region = regRow.id;
        //            }
        //        }

        //        // update name if it has changes
        //        if( vcls[row].Vehicle != row.name )
        //        {
        //            row.name = vcls[row].Vehicle;
        //        }

        //        index++;
        //    }

            

            
        //}


        // adds region and children

  
        private int getRegionId( string name )
        {
            try
            {
                return db.region.First( row => row.name == name ).id;
            }
            catch( Exception e )
            {
                string error = e.Message;
            }

            return 0;
        }


    

        //public bool UpdateVehicle( MediaVehicle vehicle )
        //{
        //    bool rval = false;
        //    string query = "id = '" + vehicle.Guid + "'";

        //    Media.vehicleRow vcl = db.vehicle.FindByid( vehicle.Guid );

        //    if( vcl != null)
        //    {
        //        rval = true;

        //        // serialize the input
        //        MemoryStream str = new MemoryStream();

        //        serializer.Serialize( str, vehicle );

        //        vcl.vehicle = str.GetBuffer();
        //    }

        //    return rval;
        //}

        //// we assume that regions have been added already
        //public void AddVehicle(MediaVehicle vehicle)
        //{
        //    string region = vehicle.RegionName;
        //    string subtype = vehicle.SubType;
        //    string media = vehicle.Type.ToString();

        //    int mediaId = addMedia( media );
        //    int subTypeId = addMediaSubType( mediaId, subtype );
        //    int regionId = getRegionId( region );

        //    Media.vehicleRow row = db.vehicle.NewvehicleRow();

        //    row.subtype = subTypeId;
        //    row.region = regionId;
        //    row.id = vehicle.Guid;
        //    row.name = vehicle.Vehicle;
        //    row.size = 0;

        //     // serialize the input
        //    MemoryStream str = new MemoryStream();

        //    serializer.Serialize( str, vehicle );

        //    row.vehicle = str.GetBuffer();

        //    db.vehicle.AddvehicleRow( row );
        //}

        // media cpm
        //public void WriteCPM(Dictionary<Media.vehicleRow, MediaVehicle> vcls)
        //{
        //    Dictionary<Media.mediaRow, double> mediaCPM = new Dictionary<Media.mediaRow, double>();
          
        //      foreach( Media.mediaRow media in db.media )
        //      {
        //         media.cpm = 0;
        //      }

        //    foreach(Media.vehicleRow vclRow in vcls.Keys)
        //    {
        //        double cpm = vcls[vclRow].CPM;
                

        //        // find media and update
        //        Media.mediaRow media = vclRow.media_subtypeRow.mediaRow;

        //        media.cpm += cpm;
        //    }

        //    foreach( Media.mediaRow media in db.media )
        //    {
        //         double numVcls = 0;

        //            foreach( Media.media_subtypeRow subtype in media.Getmedia_subtypeRows() )
        //            {
        //                numVcls += subtype.GetvehicleRows().Length;
        //            }

        //            if( numVcls > 0 )
        //            {
        //                media.cpm = media.cpm / numVcls;
        //            }
        //            else
        //            {
        //                media.cpm = 0;
        //            }
        //    }
        //}

    
        private int addMedia( string name )
        {

            if( db.media.LongCount(row => row.name == name ) > 0 )
            {
                return db.media.First( row => row.name == name ).id;
            }

            Media.mediaRow media = db.media.NewmediaRow();

            media.name = name;
            media.cpm = 0;

            db.media.AddmediaRow( media );

            return media.id;
        }

        private int addMediaSubType( int mediaID, string name )
        {
            if(db.media_subtype.LongCount( row => (row.name == name) && (row.media == mediaID)) > 0)
            {
                return  db.media_subtype.First( row => (row.name == name) && (row.media == mediaID)).id;
            }

            Media.media_subtypeRow subType = db.media_subtype.Newmedia_subtypeRow();

            subType.media = mediaID;

            subType.name = name;

            db.media_subtype.Addmedia_subtypeRow( subType );

            return subType.id;
        }

        //public void BuildAdOptionTable()
        //{
        //    //db.ad_options.Clear();
        //    //Dictionary<string, Dictionary<string, int>> num_options = new Dictionary<string, Dictionary<string, int>>();
        //    //Dictionary<string, Dictionary<string, SimpleOption>> average_options = new Dictionary<string, Dictionary<string, SimpleOption>>();
        //    //Dictionary<string, Dictionary<string, AdOption>> other_options = new Dictionary<string, Dictionary<string, AdOption>>();

        //    //foreach (Media.vehicleRow vehicle_row in db.vehicle.Rows)
        //    //{
        //    //    MemoryStream str = new MemoryStream(vehicle_row.vehicle);
        //    //    MediaVehicle vehicle = (MediaVehicle)serializer.Deserialize(str);

        //    //    string type = vehicle.Type.ToString();

        //    //    if (!num_options.ContainsKey(type))
        //    //    {
        //    //        num_options.Add(type, new Dictionary<string, int>());
        //    //        average_options.Add(type, new Dictionary<string, SimpleOption>());
        //    //        other_options.Add(type, new Dictionary<string, AdOption>());
        //    //    }

        //    //    foreach (AdOption option in vehicle.GetOptions().Values)
        //    //    {
        //    //        string id = option.ID;
        //    //        SimpleOption simple_option = option as SimpleOption;
        //    //        if (simple_option != null)
        //    //        {
        //    //            if (!average_options[type].ContainsKey(id))
        //    //            {
        //    //                average_options[type].Add(id, simple_option);
        //    //                num_options[type].Add(id, 1);
        //    //            }
        //    //            else
        //    //            {
        //    //                average_options[type][id] = add_options(average_options[type][id], simple_option);
        //    //                num_options[type][id]++;
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            if (!average_options[type].ContainsKey(id))
        //    //            {
        //    //                other_options[type].Add(id, option);
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //foreach (int type in num_options.Keys)
        //    //{
        //    //    foreach (string id in num_options[type].Keys)
        //    //    {
        //    //        divide_option(average_options[type][id], (double)num_options[type][id]);
        //    //    }
        //    //}

        //    //foreach (int type in average_options.Keys)
        //    //{
        //    //    int type_id = (int)db.media.Select("name = '" + type + "'")[0]["id"];
        //    //    foreach (string id in average_options[type].Keys)
        //    //    {
        //    //        Media.ad_optionsRow option_row = db.ad_options.Newad_optionsRow();
        //    //        option_row.id_tag = id;
        //    //        option_row.type_id = type_id;
        //    //        option_row.name = average_options[type][id].Name;

        //    //        MemoryStream str = new MemoryStream();
        //    //        serializer.Serialize(str, average_options[type][id]);
        //    //        option_row.ad_option = str.GetBuffer();
        //    //        db.ad_options.Addad_optionsRow(option_row);
        //    //    }

        //    //    foreach (string id in other_options[type].Keys)
        //    //    {
        //    //        Media.ad_optionsRow option_row = db.ad_options.Newad_optionsRow();
        //    //        option_row.id_tag = id;
        //    //        option_row.type_id = type_id;
        //    //        option_row.name = other_options[type][id].Name;

        //    //        MemoryStream str = new MemoryStream();
        //    //        serializer.Serialize(str, other_options[type][id]);
        //    //        option_row.ad_option = str.GetBuffer();
        //    //        db.ad_options.Addad_optionsRow(option_row);
        //    //    }
        //    //}

        //    //Update();
        //}

        //private SimpleOption add_options(SimpleOption option1, SimpleOption option2)
        //{
        //    SimpleOption rval = new SimpleOption(option1.Name, option1.ID, option1.Prob_Per_Hour + option2.Prob_Per_Hour, option1.Awareness + option2.Awareness, option1.Persuasion + option2.Persuasion, option1.Recency + option2.Recency, option1.Cost_Modifier + option2.Cost_Modifier);
        //    return rval;
        //}

        //private void divide_option(SimpleOption option, double demon)
        //{
        //    option.Awareness /= demon;
        //    option.Persuasion /= demon;
        //    option.Recency /= demon;
        //    option.Prob_Per_Hour /= demon;
        //    option.Cost_Modifier /= demon;
        //}

        //public void BuildVehicleOptions(List<MediaVehicle> vehicles)
        //{
        //    db.vehicle_options.Clear();

        //    foreach (MediaVehicle vehicle in vehicles)
        //    {
        //        Guid guid = vehicle.Guid;
        //        foreach (int option in vehicle.GetOptions().Keys)
        //        {
        //            DataRow[] rows = db.ad_options.Select("id = '" + option + "'");
        //            if (rows.Length != 1)
        //            {
        //                continue;
        //            }
        //            int id = (int)rows[0]["id"];
        //            Media.vehicle_optionsRow row = db.vehicle_options.Newvehicle_optionsRow();
        //            row.vehicle_id = guid;
        //            row.option_id = id;
        //            db.vehicle_options.Addvehicle_optionsRow(row);
        //        }
        //    }

        //    Update();
        //}

        public void NewOption(int type_id, AdOption option)
        {
            Media.ad_optionsRow row = db.ad_options.Newad_optionsRow();
            row.type_id = type_id;
            row.id_tag = option.ID.ToString();
            row.name = option.Name;
            MemoryStream str = new MemoryStream();
            serializer.Serialize(str, option);
            row.ad_option = str.GetBuffer();

            db.ad_options.Addad_optionsRow(row);

        }

        #endregion

        #region Agents And vehicle stats

        private double numAgents = 0;
      
        // region -> vec
        private Dictionary<int, Demographic.PrimVector> regionDemoStore = new Dictionary<int, Demographic.PrimVector>();

        // media -> media subtype -> region -> vec
       private Dictionary<string, Dictionary<string, Dictionary<string, Demographic.PrimVector>>> mediaDemoStore = new Dictionary<string, Dictionary<string, Dictionary<string, Demographic.PrimVector>>>();


       public void AddAgentsToRegions( List<Agent> agents )
       {
           regionDemoStore.Clear();

           // initialize temporary storage
           foreach( Media.regionRow region in db.region )
           {
               regionDemoStore.Add( region.id, new Demographic.PrimVector() );
           }

           numNoMatch = 0;
           foreach( Agent agent in agents )
           {
               // update region_demographics

               updateRegionDemographics( agent );
           }

           // get id of top most region
           Media.regionRow topGeo = db.region.First( row => row.Getregion_treeRowsByFK_region_tree_child_region().Length == 0);

           numAgents = regionDemoStore[topGeo.id].Any;

           if( numAgents > 0 )
           {
               normalizeRegionDemographics( 1.0 / numAgents );
           }

           // region
           foreach( Media.regionRow region in db.region )
           {
               region.demo = primVecToBytes( regionDemoStore[region.id] );
           }

           regionDemoStore.Clear();
       }

        public void NormalizeVehicleDemographics(double num )
        {
            if( num > 0 )
            {
                normalizeDemographics(1.0 / num );
            }

            writeStoreToTables();
        }

        private void updateRegionDemographics( Agent agent )
        {
            // first find region row for this agent
            if(!db.region.Any( row => row.geo_id == agent.House.GeoID ) )
            {
                return;
            }

            Media.regionRow reg = db.region.First( row => row.geo_id == agent.House.GeoID );
            List<Media.regionRow> allRegs = new List<Media.regionRow>();

            allRegs.Add( reg );


            ancestors( reg, allRegs );

            foreach( Media.regionRow anc in allRegs )
            {

                Demographic.PrimVector vec = regionDemoStore[anc.id];

                bool match = false;
                for( int ii = 0; ii < Demographic.PrimitiveTypes.Count; ++ii )
                {
                    if( agent.House.Match( Demographic.PrimitiveTypes[ii] ) )
                    {
                        vec[ii] += 1;

                        match = true;
                    }
                }

                if( match )
                {
                    vec.Any += 1;
                }
                else
                {
                    numNoMatch++;
                }
            }
        }

        // similar to region except we also filter and decompose by vehicle
        int numNoMatch = 0;

        private void normalizeRegionDemographics( double scale )
        {
            // region demographics
            foreach( int id in regionDemoStore.Keys )
            {
                Demographic.PrimVector vec = regionDemoStore[id];

                vec.Any *= scale;

                for( int ii = 0; ii < vec.Count; ++ii )
                {
                    vec[ii] *= scale;
                }
            }
        }

        private void normalizeDemographics(double scale)
        {
            if( Agent.KeepStats )
            {
                //  media demographics
                foreach( Media.mediaRow media in db.media )
                {
                    if( Agent.MediaDemoStore.ContainsKey( media.name ) )
                    {
                        foreach( Media.media_subtypeRow subType in media.Getmedia_subtypeRows() )
                        {
                            if( Agent.MediaDemoStore[media.name].ContainsKey( subType.name ) )
                            {
                                foreach( Demographic.PrimVector vec in Agent.MediaDemoStore[media.name][subType.name].Values )
                                {
                                    vec.Any *= scale;

                                    for( int ii = 0; ii < vec.Count; ++ii )
                                    {
                                        vec[ii] *= scale;
                                    }
                                }
                            }
                        }
                    }
                }


                foreach( Media.vehicleRow vcl in db.vehicle )
                {
                    if( Agent.VehicleSize.ContainsKey( vcl.id ) )
                    {
                        Agent.VehicleSize[vcl.id] *= scale;
                    }
                }
            }
        }
              

        private void writeStoreToTables()
        {
            if( Agent.KeepStats )
            {
                //
                // Coded this up as an alternative way to do this
                // But it should be faster to loop through all the media type, subtypes and regions
                // and then check if it is in the smaller store
                // then it would be to loop through the smaller store and 
                // then look for the rows in the larger tables
                //
                //foreach( string media in Agent.MediaDemoStore.Keys )
                //{
                //    foreach( string subtype in Agent.MediaDemoStore[media].Keys )
                //    {
                //        foreach( string region in Agent.MediaDemoStore[media][subtype].Keys )
                //        {
                //            try
                //            {
                //                Media.media_subtypeRow subtypeRow = db.media_subtype.First( row => row.name == subtype &&
                //                    row.mediaRow.name == media );

                //                Media.regionRow regionRow = db.region.First( row => row.name = region );


                //                // create media demographic
                //                Media.media_demographicRow media_demo = db.media_demographic.Newmedia_demographicRow();

                //                media_demo.region = regionRow.id;

                //                media_demo.subtype = subtypeRow.id;

                //                media_demo.demo = primVecToBytes( Agent.MediaDemoStore[media][subtype][region] );

                //                db.media_demographic.Addmedia_demographicRow( media_demo );
                //            }
                //            catch( Exception )
                //            {
                //                continue;
                //            }
                //        }
                //    }
                //}


                // need to also add thre appropriate items to the media_demographic table
                foreach( Media.regionRow reg in db.region )
                {
                    foreach( Media.media_subtypeRow subType in db.media_subtype )
                    {
                        string region = reg.name;
                        string subtype = subType.name;
                        string media = subType.mediaRow.name;

                        // only if it is in the store do we add to the table
                        if( Agent.MediaDemoStore.ContainsKey( media ) &&
                            Agent.MediaDemoStore[media].ContainsKey( subtype ) &&
                            Agent.MediaDemoStore[media][subtype].ContainsKey( region ) )
                        {
                            
                            Media.media_demographicRow media_demo = db.media_demographic.Newmedia_demographicRow();

                            media_demo.subtype = subType.id;
                            media_demo.region = reg.id;

                            media_demo.demo = primVecToBytes( Agent.MediaDemoStore[media][subtype][region] );

                            db.media_demographic.Addmedia_demographicRow( media_demo );
                        }
                    }
                }

                // this might be faster looping through the store and searching for the id in the larger table
                // because we are searching for a primary key
                foreach( Guid guid in Agent.VehicleSize.Keys )
                {
                    Media.vehicleRow vcl = db.vehicle.FindByid( guid );
                    foreach( Media.vehicle_regionRow vclReg in vcl.Getvehicle_regionRows() )
                    {
                        // TBD - we need to keep stats on vehicle by city
                       // vcl.size = Agent.VehicleSize[guid];
                    }
                }


                //foreach( Media.vehicleRow vcl in db.vehicle )
                //{
                //    if( Agent.VehicleSize.ContainsKey( vcl.id ) )
                //    {
                //        vcl.size = Agent.VehicleSize[vcl.id];
                //    }
                //}
            }
        }

        #endregion

        #region utility
        private Demographic.PrimVector bytesToPrimVec(byte[] arr)
        {
            
            byte[] decompArr = StreamUtilities.Decompress( arr );

            MemoryStream outStr = new MemoryStream(decompArr);

            try
            {
                return (Demographic.PrimVector)serializer.Deserialize( outStr );
            }
            catch( Exception )
            {
                return new Demographic.PrimVector();
            }
        }

        private  byte[] primVecToBytes( Demographic.PrimVector vec)
        {
            MemoryStream inStr = new MemoryStream();

            serializer.Serialize( inStr, vec );

            return StreamUtilities.Compress( inStr.GetBuffer() );
        }
        #endregion

      

        #region  Media Info for UI

        public static int US_HH_SIZE
        {
            get
            {
                return 126000000;
            }
        }


        /// <summary>
        /// List of subtypes for a given media type
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public List<string> GetSubTypes( string mediaType )
        {
             
            List<string> rval = new List<string>();

            
            foreach(Media.media_subtypeRow subtype in db.media_subtype.Where(row => row.mediaRow.name == mediaType))
            {
                    rval.Add(subtype.name);
            }
           

            return rval;
        }

        public bool Initialized
        {
            get
            {
                return db.media.Count > 0;
            }
        }

        /// <summary>
        /// List of current media types
        /// </summary>
        /// <returns></returns>
        public List<string> GetMediaTypes()
        {
            List<string> rval = new List<string>();

            foreach( Media.mediaRow media in db.media )
            {
                rval.Add( media.name );
            }

            return rval;
        }

        public int GetTypeID(string type_name)
        {
            return db.media.First( row => row.name.ToLower() == type_name.ToLower() ).id;
        }

        public MediaVehicle.MediaType GetTypeForID(int id)
        {
            return MediaVehicle.GetType(db.media.FindByid(id).name);
        }

        public Dictionary<string, double> GetTypeCPM()
        {
            Dictionary<string, double> rval = new Dictionary<string, double>();

            foreach( Media.mediaRow media in db.media )
            {
                rval.Add( media.name, media.cpm );
            }

            return rval;
        }

        public int GetSubtypeID(int type_id, string subtype_name)
        {
            DataRow[] rows = db.media_subtype.Select("media = " + type_id + " AND name = '" + subtype_name + "'");
            return (int)rows[0]["id"];
        }

        ///// <summary>
        ///// return vehicle matching Guid
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public MediaVehicle GetVehicle( Guid id, bool get_options )
        //{
        //    object obj;

        //    System.Data.SqlClient.SqlCommand genericCommand = new System.Data.SqlClient.SqlCommand();
        //    genericCommand.Connection = new System.Data.SqlClient.SqlConnection( connectionString );

        //    genericCommand.CommandText = "SELECT vehicle FROM vehicle WHERE id = '" + id + "'";
        //    genericCommand.Connection.Open();

        //    try
        //    {
        //        obj = genericCommand.ExecuteScalar();
        //    }
        //    catch( Exception )
        //    {


        //        genericCommand.Connection.Close();

        //        return null;
        //    }

        //    genericCommand.Connection.Close();

        //    byte[] arr = (byte[])obj;

        //    MemoryStream str = new MemoryStream( arr );

        //    MediaVehicle vcl = (MediaVehicle)serializer.Deserialize( str );

        //    if (get_options)
        //    {
        //        RefreshVehicleOptions(vcl);
        //    }

        //    return vcl;
        //}

        public Demographic.PrimVector TargetPopulation(List<Demographic> segments, GeoRegion region)
        {
            List<GeoRegion> region_list = new List<GeoRegion>();
            region_list.Add(region);

            return TargetPopulation(segments, region_list);
        }

        /// <summary>
        /// Decompose segments into primitive demographics
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public Demographic.PrimVector TargetPopulation( List<Demographic> segments, List<GeoRegion> geoRegions )
        {
            Demographic.PrimVector rval = new Demographic.PrimVector();

            if( segments.Count == 0 )
            {
                return rval;
            }

            // OR of all segments
            Demographic comp = segments[0];

            for( int ii = 1; ii < segments.Count; ++ii )
            {
                comp = comp | segments[ii];
            }

            Demographic.PrimVector compVec = comp.ToPrimVector();

            // use db ro compute
            System.Data.SqlClient.SqlCommand genericCommand = new System.Data.SqlClient.SqlCommand();
            genericCommand.Connection = new System.Data.SqlClient.SqlConnection( connectionString );
            genericCommand.CommandTimeout = 0;

            foreach( GeoRegion reg in geoRegions )
            {
                genericCommand.CommandText = "SELECT demo from region where id = " + reg.Id;

                genericCommand.Connection.Open();
                System.Data.SqlClient.SqlDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );
                object[] demoData = new object[2];
                while( dataReader.Read() )
                {
                    byte[] demo = (byte[])dataReader.GetValue( 0 );

                    Demographic.PrimVector vec = bytesToPrimVec( demo );

                    rval.Any += vec.Any;

                    // product of PrimVector
                    for( int ii = 0; ii < vec.Count; ++ii )
                    {
                        rval[ii] += compVec[ii] * vec[ii];
                    }
                }

                dataReader.Close();
               
            }

            return rval;
        }

        public List<string> GetSubTypes( string mediaType, List<GeoRegion> geoRegions )
        {
            List<string> rval = new List<string>();

            Dictionary<string, Demographic.PrimVector> typeDict = GetSubTypeDemographics( mediaType, geoRegions );

            foreach( string type in typeDict.Keys )
            {
                rval.Add( type );
            }

            return rval;
        }

        /// <summary>
        /// compute a list of  mediaSubtypes (as a string) and their demographic size
        /// relative to a given media type and region
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public Dictionary<string, Demographic.PrimVector> GetSubTypeDemographics(string mediaType, List<GeoRegion> geoRegions)
        {
            System.Data.SqlClient.SqlCommand genericCommand = new System.Data.SqlClient.SqlCommand();
            genericCommand.Connection = new System.Data.SqlClient.SqlConnection( connectionString );
            genericCommand.CommandTimeout = 0;

            Dictionary<string,Demographic.PrimVector> rval = new Dictionary<string,Demographic.PrimVector>();

            List<Media.regionRow> regionRows = this.Relations( geoRegions );

            object[] vclData = new object[2];
            foreach( Media.regionRow regRow in regionRows )
            {
                genericCommand.CommandText = "SELECT media_subtype.name, media_demographic.demo " +
                    " FROM media_demographic, media_subtype, media " +
                    " WHERE media_demographic.region = " + regRow.id +
                    " AND media_demographic.subtype = media_subtype.id " +
                    " AND media_subtype.media = media.id " +
                    " AND media.name = '" + mediaType + "'";

                genericCommand.Connection.Open();
                System.Data.SqlClient.SqlDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

                while( dataReader.Read() )
                {
                    dataReader.GetValues( vclData );

                    string subtype = (string)vclData[0];

                    // only add once
                    if( !rval.ContainsKey( subtype ) )
                    {
                        byte[] arr = (byte[])vclData[1];

                        Demographic.PrimVector vec = bytesToPrimVec( arr );

                        rval.Add( subtype, vec );
                    }
                }

                dataReader.Close();
            }

            return rval;
        }

        private struct MediaSize
        {
            public MediaVehicle Vehicle;
            public double Size;
        }

        private void ancestors(Media.regionRow region,  List<Media.regionRow> rval )
        {
            Media.region_treeRow[] parents = region.Getregion_treeRowsByFK_region_tree_child_region();

            foreach( Media.region_treeRow tree in parents )
            {
                Media.regionRow parent = tree.regionRowByFK_region_tree_parent_region;

                if( rval.Contains( parent ) )
                {
                    continue;
                }

                rval.Add( parent );

                ancestors( parent, rval );
            }
        }

        private void descendents( Media.regionRow region, List<Media.regionRow> rval )
        {
            Media.region_treeRow[] parents = region.Getregion_treeRowsByFK_region_tree_parent_region();

            if( parents.Length > 0 )
            {
                foreach( Media.region_treeRow tree in parents )
                {
                    Media.regionRow child = tree.regionRowByFK_region_tree_child_region;

                    if( rval.Contains( child ) )
                    {
                        continue;
                    }

                    rval.Add( child );

                    descendents( child, rval );
                }
            }
            else
            {
                // check if children in db not read in
                MediaLibrary.MediaTableAdapters.regionTableAdapter regAdpater = new MediaLibrary.MediaTableAdapters.regionTableAdapter();
                regAdpater.Connection = new System.Data.SqlClient.SqlConnection( connectionString );
                using( Media.regionDataTable tbl = regAdpater.GetRegionByParent( region.id ) )
                {

                    foreach( Media.regionRow subreg in tbl )
                    {
                        rval.Add( subreg );

                        descendents( subreg, rval );
                    }
                }
            }
        }

        private List<Media.regionRow> Relations( List<GeoRegion> geoRegions )
        {
            List<Media.regionRow> rval = new List<Media.regionRow>();

            foreach( GeoRegion geo in geoRegions )
            {
                Media.regionRow region = db.region.First( row => row.name == geo.Name );

                if( rval.Contains( region ) )
                {
                    continue;
                }

                rval.Add( region );
            }

            foreach( GeoRegion geo in geoRegions )
            {
                Media.regionRow region = db.region.First( row => row.name == geo.Name );

                descendents( region, rval );
            }

            foreach( GeoRegion geo in geoRegions )
            {
                Media.regionRow region = db.region.First( row => row.name == geo.Name );

                ancestors( region, rval );
            }

            return rval;
        }

        /// <summary>
        /// Find vehicles matching a media type & subtype within a region
        /// each vehicle has a size representing the number of subscribers
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaSubYype"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private List<MediaVehicle> GetMediaVehicleDetail( string mediaType, string mediaSubType, List<GeoRegion> geoRegions )
        {
            MediaLibrary.MediaTableAdapters.vehicleTableAdapter vclAdpater = new MediaLibrary.MediaTableAdapters.vehicleTableAdapter();
            MediaLibrary.MediaTableAdapters.vehicle_regionTableAdapter regAdpater =  new MediaLibrary.MediaTableAdapters.vehicle_regionTableAdapter();
            vclAdpater.Connection = new System.Data.SqlClient.SqlConnection( connectionString );
            regAdpater.Connection = new System.Data.SqlClient.SqlConnection( connectionString );

            List<MediaVehicle> rval = new List<MediaVehicle>();


            Media.media_subtypeRow subtype = null;

            if( mediaSubType != null )
            {
                subtype = db.media_subtype.First( row => row.name == mediaSubType && row.mediaRow.name == mediaType );
            }


            Media.mediaRow type = db.media.First( row => row.name == mediaType);
            



            // we get all related regions
            // this list is contains in order
            // regions correspondgin to the list of GeoRegions
            // regions contained in a region in the list of GeoRegions
            // regions containing in a region in the list of GeoRegions
            //
            // Since we only add a vehicle once this has the effect of preselecting
            // on vehicles that are in the
            // regions desired
            // smaller regions
            // larger regions
            List<Media.regionRow> regionRows = this.Relations( geoRegions );

            foreach( Media.regionRow regionRow in regionRows )
            {
                Media tmpDb = new Media();

                tmpDb.EnforceConstraints = false;

                vclAdpater.FillFromRegion( tmpDb.vehicle, regionRow.id, type.id );
                regAdpater.FillByRegion( tmpDb.vehicle_region, regionRow.id, type.id );

                foreach( Media.vehicleRow vcl in tmpDb.vehicle )
                {
                    if( subtype == null || vcl.subtype == subtype.id )
                    {
                        if( rval.Any( row => row.Vehicle == vcl.name ) )
                        {
                            // we only add a given vehicle once
                            continue;
                        }

                        int type_id = db.media_subtype.FindByid( vcl.subtype ).media;
                        string subtypeName = db.media_subtype.FindByid( vcl.subtype ).name;
                        string regionName = regionRow.name;
                        double size = US_HH_SIZE * vcl.Getvehicle_regionRows().Where( row => row.region_id == regionRow.id ).Average( row => row.size );

                        MediaVehicle media = new MediaVehicle( vcl, GetTypeForID( type_id ), subtypeName, regionName, size, GetTypeOptions( type_id ) );

                        IEnumerable<Media.anti_vcl_optionRow> exclude = excludedOptions( vcl.id );
                        if( exclude != null )
                        {
                            media.ExcludeOptions( exclude );
                        }

                        // update 
                        rval.Add( media );
                    }
                }
            }

            return rval;
        }


        /// <summary>
        /// Find vehicles matching a media type & subtype within a region
        /// each vehicle has a size representing the number of subscribers
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaSubYype"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<MediaVehicle> GetMediaVehicleWithSize( string mediaType, string mediaSubYype, List<GeoRegion> geoRegions )
        {
            List<MediaVehicle> rval = GetMediaVehicleDetail( mediaType, mediaSubYype, geoRegions );

            if( GetTypeForID( GetTypeID( mediaType ) ) == MediaVehicle.MediaType.Magazine )
            {
                List<Demographic> everybody = new List<Demographic>();
                everybody.Add( new Demographic() );
                double region_size = TargetPopulation( everybody, geoRegions ).Any;

                foreach( MediaVehicle vcl in rval )
                {
                    if( vcl.RegionName == "USA" )
                    {
                        // scales to region if vcl can target DMAs
                        vcl.AdjustSize( region_size );
                    }
                }
            }

            return rval;
        }



        /// <summary>
        /// Find vehicles matching a media type & subtype within a region
        /// each vehicle has a size representing the number of subscribers
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaSubYype"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<MediaVehicle> GetMediaVehicleWithSize( string mediaType, List<GeoRegion> geoRegions )
        {
            return GetMediaVehicleWithSize( mediaType, null, geoRegions );
        }

        private IEnumerable<Media.anti_vcl_optionRow> excludedOptions( Guid vcl_id )
        {
            if( db.anti_vcl_option.Any( row => row.vehicle_id == vcl_id ) )
            {
                return db.anti_vcl_option.Where( row => row.vehicle_id == vcl_id );
            }

            return null;
        }

        public Dictionary<int, Dictionary<int, AdOption>> GetAllOptions()
        {
            if( type_options != null )
            {
                return type_options;
            }

            type_options = new Dictionary<int, Dictionary<int, AdOption>>();

            foreach( Media.ad_optionsRow row in db.ad_options )
            {
                int type_id = row.type_id;
                if( !type_options.ContainsKey( type_id ) )
                {
                    type_options.Add( type_id, new Dictionary<int, AdOption>() );
                }


                MemoryStream str = new MemoryStream( row.ad_option );
                AdOption option = (AdOption)serializer.Deserialize( str );
                type_options[type_id].Add( row.id, option );
            }

            return type_options;
        }

        public Dictionary<int, AdOption> GetTypeOptions(int type_id)
        {
            return GetAllOptions()[type_id];
        }

        public AdOption GetAdOption(int id)
        {
            foreach( Dictionary<int, AdOption> dict in GetAllOptions().Values )
            {
                if( dict.ContainsKey( id ) )
                {
                    return dict[id];
                }
            }

            return null;
        }

        public string GetVehicleName( Guid id )
        {
            string rval = "Name Not Available";

            try
            {
                MediaLibrary.MediaTableAdapters.vehicleTableAdapter adapt = new MediaLibrary.MediaTableAdapters.vehicleTableAdapter();

                adapt.Connection = new System.Data.SqlClient.SqlConnection( connectionString );

                rval = adapt.VehicleName( id );
            }
            catch( Exception e )
            {
                rval = e.Message;
            }

            return rval;
        }

        #endregion


        #region categories

        public void ClearCategoryData()
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            command.Connection = new System.Data.SqlClient.SqlConnection(adaptMan.Connection.ConnectionString);

            command.Connection.Open();
            command.CommandText = "delete from business_category";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from business_subcategory";
            command.ExecuteNonQuery();
            command.Connection.Close();

            command.Connection.Open();
            command.CommandText = "delete from category_subtype";
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public void AddCategory(string category)
        {
            Media.business_categoryRow row = db.business_category.Newbusiness_categoryRow();
            row.category = category;
            db.business_category.Addbusiness_categoryRow(row);
        }

        public bool AddSubCategory(string subcategory, string category)
        {
            DataRow[] rows = db.business_category.Select("category = '" + category + "'");
            if (rows.Length != 1)
            {
                return false;
            }
            int cat_id = (int)rows[0]["id"];

            Media.business_subcategoryRow sub_row = db.business_subcategory.Newbusiness_subcategoryRow();
            sub_row.cat_id = cat_id;
            sub_row.subcategory = subcategory;
            db.business_subcategory.Addbusiness_subcategoryRow(sub_row);

            return true;
        }

        public bool AddCategorySubtype(string subtype, string subcategory)
        {
            //warning if moving to mySQL the escape character needs to be changed to \t
            string clean = subcategory.Replace("'", "''");
            DataRow[] rows = db.business_subcategory.Select("subcategory = '" + clean + "'");
            if (rows.Length != 1)
            {
                return false;
            }
            int subcat_id = (int)rows[0]["id"];

            rows = db.media_subtype.Select("name = '" + subtype + "'");
            if (rows.Length != 1)
            {
                return false;
            }
            int subtype_id = (int)rows[0]["id"];

            Media.category_subtypeRow catsubtype_row = db.category_subtype.Newcategory_subtypeRow();
            catsubtype_row.subcat_id = subcat_id;
            catsubtype_row.subtype_id = subtype_id;

            db.category_subtype.Addcategory_subtypeRow(catsubtype_row);

            return true;
        }

        public Dictionary<string, int> GetCategories()
        {
            Dictionary<string, int> categories = new Dictionary<string, int>();
            foreach (Media.business_categoryRow row in db.business_category.Rows)
            {
                categories.Add(row.category, row.id);
            }
            return categories;
        }

        public Dictionary<string, int> GetSubCategories(int category)
        {
            Dictionary<string, int> subcategories = new Dictionary<string,int>();
            DataRow[] rows = db.business_subcategory.Select("cat_id = " + category);
            foreach (DataRow row in rows)
            {
                subcategories.Add((string)row["subcategory"], (int)row["id"]);
            }
            return subcategories;
        }

        

        public Dictionary<string, int> GetSubTypes(int subcategory)
        {
            Dictionary<string, int> subtypes = new Dictionary<string, int>();
            DataRow[] rows = db.category_subtype.Select("subcat_id = " + subcategory);
            foreach (DataRow row in rows)
            {
                int subtype_id = (int)row["subtype_id"];
                DataRow[] subtype_rows = db.media_subtype.Select("id = " + subtype_id);
                if (subtype_rows.Length != 1)
                {
                    continue;
                }
                string subtype_name = (string)subtype_rows[0]["name"];
                subtypes.Add(subtype_name, subtype_id);
            }
            return subtypes;
        }

        public Dictionary<string, int> GetSubTypes(List<int> subcategories)
        {
            Dictionary<string, int> subtypes = new Dictionary<string, int>();
            foreach (int subcategory in subcategories)
            {
                DataRow[] rows = db.category_subtype.Select("subcat_id = " + subcategory);
                foreach (DataRow row in rows)
                {
                    int subtype_id = (int)row["subtype_id"];
                    DataRow[] subtype_rows = db.media_subtype.Select("id = " + subtype_id);
                    if (subtype_rows.Length != 1)
                    {
                        continue;
                    }
                    string subtype_name = (string)subtype_rows[0]["name"];
                    subtypes.Add(subtype_name, subtype_id);
                }
            }
            return subtypes;
        }

        #endregion

    }
}
