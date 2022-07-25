﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using NetFwTypeLib;

namespace HeuristicLab.RemoteControl.TestPlugin {

  public static class FirewallPolicyExtensions {
    public static IRestServer UseFirewallPolicy(this IRestServer server, FirewallPolicy policy) {
      server.AfterStarting += policy.AddFirewallPolicy;
      server.BeforeStopping += policy.RemoveFirewallPolicy;
      return server;
    }
  }
  public class FirewallPolicy {
    private INetFwRule _rule;
    private INetFwPolicy2 _policy;

    public string AppExecutablePath { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public INetFwPolicy2 Policy => _policy;

    public INetFwRule Rule => _rule;

    private INetFwRule GenerateRule() {
      if (_policy == null)
        _policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

      _rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
      _rule.ApplicationName = AppExecutablePath;
      _rule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
      _rule.Description = Description;
      _rule.Enabled = true;
      _rule.InterfaceTypes = "All";
      _rule.Name = Name;

      return _rule;
    }

    public void AddFirewallPolicy(IRestServer server) {
      if (_rule != null) return;
      _policy.Rules.Add(GenerateRule());
    }

    public void RemoveFirewallPolicy(IRestServer server) {
      if (_rule == null) return;
      _policy.Rules.Remove(_rule.Name);
      _rule = null;
    }
  }
}
