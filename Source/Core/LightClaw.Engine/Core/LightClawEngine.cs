﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightClaw.Engine.Configuration;
using LightClaw.Engine.Graphics;
using LightClaw.Engine.IO;
using LightClaw.Extensions;
using log4net;
using Munq;

namespace LightClaw.Engine.Core
{
    public class LightClawEngine
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LightClawEngine));

        private static readonly IocContainer _DefaultIocContainer = new IocContainer();

        public static IocContainer DefaultIocContainer
        {
            get
            {
                return _DefaultIocContainer;
            }
        }

        static LightClawEngine()
        {
            DefaultIocContainer.Register<IContentManager>(d => new ContentManager());
            //AppDomain.CurrentDomain.ProcessExit += (s, e) => LogManager.Shutdown();
        }

        static void Main(string[] args)
        {
            logger.Info("Starting engine...");

            try
            {
                Contract.Assume(GeneralSettings.Default.StartScene != null);
                Contract.Assume(GeneralSettings.Default.GameCodeAssembly != null);
                using (IGame game = new Game(Assembly.LoadFrom(GeneralSettings.Default.GameCodeAssembly), GeneralSettings.Default.StartScene) { Name = GeneralSettings.Default.GameName })
                {
                    DefaultIocContainer.Register<IGame>(d => game);
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("An error of type '{0}' with message '{1}' occured.".FormatWith(ex.GetType().AssemblyQualifiedName, ex.Message), ex);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
