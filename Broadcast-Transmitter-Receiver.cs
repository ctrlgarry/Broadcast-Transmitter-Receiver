#region Prelude
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

// Change this namespace for each script you create.
//namespace SpaceEngineers.UWBlockPrograms.BatteryMonitor {


//Приёмник
IMyBroadcastListener Antenna1; //обьявляем слушателя
string tag = "chanel"; // для удобства выведен тег
MyIGCMessage message; //обьект сообщений
IMyTextPanel panel; // обьект текстовая панель
List<IMyTerminalBlock> doors;
public Program ()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update10;// запуск каждые 10 тиков
    Antenna1 = IGC.RegisterBroadcastListener(tag); // типо частоты, слышат только те, у кого такой тег
    message = new MyIGCMessage();
    panel = GridTerminalSystem.GetBlockWithName("LCD") as IMyTextPanel; // обьявляем панельку
    doors = new List<IMyTerminalBlock>(); //doors это терминал
    GridTerminalSystem.GetBlocksOfType<IMyAirtightHangarDoor>(doors); //все ангарные двери грида
}
public void Main(string args)
{
    if (Antenna1.HasPendingMessage) // если пришло смс
    {
        message = Antenna1.AcceptMessage();// записываем смс
        panel.WriteText(Convert.ToString(message.Data)); // превращаем данные в стринги

        if (Convert.ToString(message.Data) == "close")// если получено смс "close"
        {
            foreach (IMyAirtightHangarDoor i in doors)
            {
                i.CloseDoor();// закрыть нахрен
            }
        }

        if (Convert.ToString(message.Data) == "open")
        {
            foreach (IMyAirtightHangarDoor i in doors)
            {
                i.OpenDoor();//открыть нахрен
            }
        }
    }
}

// передатчик
string Tag = "chanel";//определяю тег канала
public Program ()
{

}
public void Main(string args)
{
    IGC.SendBroadcastMessage<string>(Tag, args);//отправляю смс с тегом и аргументом запуска
}