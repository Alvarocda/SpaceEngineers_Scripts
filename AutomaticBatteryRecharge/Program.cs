using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.

        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set Runtime.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {

            // Configurações
            string cockpitName = "My Cockpit";        // Nome do cockpit
            string connectorName = "My Connector";   // Nome do connector
            string batteryGroupName = "My Batteries"; // Nome do grupo de baterias

            // Obter o cockpit
            IMyCockpit cockpit = GridTerminalSystem.GetBlockWithName(cockpitName) as IMyCockpit;
            if (cockpit == null)
            {
                SendMessage("Cockpit não encontrado!");
                return;
            }

            // Obter o connector
            IMyShipConnector connector = GridTerminalSystem.GetBlockWithName(connectorName) as IMyShipConnector;
            if (connector == null)
            {
                SendMessage("Connector não encontrado!");
                return;
            }

            // Obter o grupo de baterias
            IMyBlockGroup batteryGroup = GridTerminalSystem.GetBlockGroupWithName(batteryGroupName);
            if (batteryGroup == null)
            {
                SendMessage("Grupo de baterias não encontrado!");
                return;
            }

            // Lista de baterias
            List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
            batteryGroup.GetBlocksOfType(batteries);

            if (batteries.Count == 0)
            {
                SendMessage("Nenhuma bateria no grupo!");
                return;
            }

            // Verificar estado do cockpit e do connector
            bool isCockpitOccupied = cockpit.IsUnderControl;
            bool isConnectorConnected = connector.Status == MyShipConnectorStatus.Connected;

            if (!isConnectorConnected)
            {
                SendMessage("Connector não esta conectado");
            }
            // Alterar estado das baterias
            if (isCockpitOccupied)
            {
                foreach (var battery in batteries)
                {
                    battery.ChargeMode = ChargeMode.Auto;
                }
                SendMessage("Cockpit ocupado: Baterias em Auto.");
            }
            else
            {
                foreach (var battery in batteries)
                {
                    battery.ChargeMode = ChargeMode.Recharge;
                }
                SendMessage("Cockpit vazio: Baterias em Recharge.");
            }
        }

        // Método para enviar mensagens ao chat
        void SendMessage(string message, string tag = "[DEBUG]")
        {
            var textMessage = $"{tag}: {message}";
            IGC.SendBroadcastMessage("ChatChannel", textMessage);
            Echo(textMessage); // Ainda exibe no terminal como referência
        }
    }
}
