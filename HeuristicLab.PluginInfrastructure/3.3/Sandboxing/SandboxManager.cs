﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2011 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using HeuristicLab.PluginInfrastructure.Manager;

namespace HeuristicLab.PluginInfrastructure.Sandboxing {
  public class SandboxManager {

    // static class
    private SandboxManager() { }

    private static StrongName CreateStrongName(Assembly assembly) {
      if (assembly == null)
        throw new ArgumentNullException("assembly");

      AssemblyName assemblyName = assembly.GetName();
      Trace.Assert(assemblyName != null, "Could not get assembly name");

      // get the public key blob
      byte[] publicKey = assemblyName.GetPublicKey();
      if (publicKey == null || publicKey.Length == 0)
        throw new InvalidOperationException("Assembly is not strongly named");

      StrongNamePublicKeyBlob keyBlob = new StrongNamePublicKeyBlob(publicKey);

      // and create the StrongName
      return new StrongName(keyBlob, assemblyName.Name, assemblyName.Version);
    }

    #region ISandboxManager Members
    public static AppDomain CreateAndInitSandbox(string appDomainName, string applicationBase, string configFilePath) {
      PermissionSet pset;

      #region permission set for sandbox
      // Uncomment code for sandboxed appdomain
      //pset = new PermissionSet(PermissionState.None);
      //pset.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
      //pset.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
      //FileIOPermission fPerm = new FileIOPermission(PermissionState.None);

      //foreach (IPluginDescription plugin in ApplicationManager.Manager.Plugins) {
      //  fPerm.AddPathList(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, plugin.Files.ToArray());
      //}

      //pset.AddPermission(fPerm);
      #endregion

      #region permission set of unrestricted appdomain
      // unrestricted appdomain 
      pset = new PermissionSet(PermissionState.Unrestricted);
      #endregion

      AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
      setup.PrivateBinPath = applicationBase;
      setup.ApplicationBase = applicationBase;
      setup.ConfigurationFile = configFilePath;

      AppDomain applicationDomain = AppDomain.CreateDomain(appDomainName, AppDomain.CurrentDomain.Evidence, setup, pset, CreateStrongName(Assembly.GetExecutingAssembly()));
      Type applicationManagerType = typeof(DefaultApplicationManager);
      DefaultApplicationManager applicationManager =
        (DefaultApplicationManager)applicationDomain.CreateInstanceAndUnwrap(applicationManagerType.Assembly.FullName, applicationManagerType.FullName, true, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
      PluginManager pm = new PluginManager(applicationBase);
      pm.DiscoverAndCheckPlugins();
      ApplicationDescription[] apps = pm.Applications.Cast<ApplicationDescription>().ToArray();
      PluginDescription[] plugins = pm.Plugins.Cast<PluginDescription>().ToArray();
      applicationManager.PrepareApplicationDomain(apps, plugins);
      return applicationDomain;
    }
    #endregion
  }
}
