using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaLibrary;

namespace Calibration
{
    class VehicleModifer
    {

        private Dictionary<int, Dictionary<int, Dictionary<Guid, MediaVehicle>>> vehicle_tree;

        public VehicleModifer(Dictionary<int, Dictionary<int, Dictionary<Guid, MediaVehicle>>> vehicle_tree)
        {
            this.vehicle_tree = vehicle_tree;
        }

        public void ModifyCPM(int type_id, int subtype_id, Guid vehicle_id, ModifyStyle mod_style, double amount)
        {
            if (type_id != -1)
            {
                ModifyCPM(vehicle_tree[type_id], subtype_id, vehicle_id, mod_style, amount);
            }
            else
            {
                foreach (int id in vehicle_tree.Keys)
                {
                    ModifyCPM(vehicle_tree[id], subtype_id, vehicle_id, mod_style, amount);
                }
            }
        }

        private void ModifyCPM(Dictionary<int, Dictionary<Guid, MediaVehicle>> vehicles, int subtype_id, Guid vehicle_id, ModifyStyle mod_style, double amount)
        {
            if (subtype_id != -1)
            {
                ModifyCPM(vehicles[subtype_id], vehicle_id, mod_style, amount);
            }
            else
            {
                foreach (int id in vehicles.Keys)
                {
                    ModifyCPM(vehicles[id], vehicle_id, mod_style, amount);
                }
            }
        }

        private void ModifyCPM(Dictionary<Guid, MediaVehicle> vehicles, Guid vehicle_id, ModifyStyle mod_style, double amount)
        {
            if (vehicle_id != null)
            {
                modify_cpm(vehicles[vehicle_id], mod_style, amount);
            }
            else
            {
                foreach (Guid id in vehicles.Keys)
                {
                    modify_cpm(vehicles[id], mod_style, amount);
                }
            }
        }

        private void modify_cpm(MediaVehicle vehicle, ModifyStyle mod_style, double amount)
        {
            Modifier.value_mod mod = Modifier.get_functor(mod_style);
           
            vehicle.CPM = mod(vehicle.CPM, amount);
        }
    }
}
