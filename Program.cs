// bat-watching/Program.cs

// see https://aka.ms/new-console-template for template info

// using System.IO;
// including System.IO namespace apparently not necessary in newer dotnet

string sysfs_Path = "/sys";

try {
  if (Directory.Exists(sysfs_Path) == false) {
    throw new FileNotFoundException(
      message: "No directory at sysfs path (‘" + sysfs_Path + "’)!"
    );
  }
} catch (Exception e) {
  Console.WriteLine("sysfs not found, sorry! Maybe you're not on a Linux OS?");
  throw e;
}

string sysfs_PowerSupplyClassPath = sysfs_Path + "/class/power_supply";
string sysfs_BatteryDeviceName = "BAT0";
string sysfs_BatteryDevicePath = sysfs_PowerSupplyClassPath + "/"
                               + sysfs_BatteryDeviceName;

// Get real battery capacity (and convert μWh from sysfs to Wh)
float batteryCapacityRealWh = Int32.Parse(
  File.ReadAllText(sysfs_BatteryDevicePath+ "/energy_full")
) / 1000000;

// Get approx factory battery capacity (and convert μWh from sysfs to Wh)
float batteryCapacityFactoryWh = Int32.Parse(
  File.ReadAllText(sysfs_BatteryDevicePath + "/energy_full_design")
) / 1000000;

float batteryHealthPercentage = (batteryCapacityRealWh / batteryCapacityFactoryWh) * 100;

Console.WriteLine("Approximate factory capacity of battery should be around:\n"
                 + batteryCapacityFactoryWh + "Wh\n"
                 + "Approximate current capacity of battery should be around:\n"
                 + batteryCapacityRealWh + "Wh\n"
                 + "This means your approximate battery health is around " + batteryHealthPercentage + "%."
                 );