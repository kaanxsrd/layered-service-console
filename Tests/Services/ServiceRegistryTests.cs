using Project.Enums;
using Project.Models;
using Project.Services;

namespace Project.Tests.Services;

public class ServiceRegistryTests : IDisposable
{
    // Reset static state before and after each test for isolation
    public ServiceRegistryTests() => ServiceRegistry.Reset();
    public void Dispose()         => ServiceRegistry.Reset();

    private static ServiceModule MakeService(string id, bool enabled = true, Role minRole = Role.User) =>
        new() { Id = id, Name = id, Description = "", IsEnabled = enabled, MinimumRole = minRole, Run = () => { } };

    // ── Register / GetAll ────────────────────────────────────────────────────

    [Fact]
    public void Register_AddsServiceToRegistry()
    {
        ServiceRegistry.Register(MakeService("svc-a"));
        Assert.Single(ServiceRegistry.GetAll());
    }

    [Fact]
    public void Register_MultipleServices_AllPresent()
    {
        ServiceRegistry.Register(MakeService("svc-a"));
        ServiceRegistry.Register(MakeService("svc-b"));
        ServiceRegistry.Register(MakeService("svc-c"));
        Assert.Equal(3, ServiceRegistry.GetAll().Count);
    }

    // ── Enable / Disable ─────────────────────────────────────────────────────

    [Fact]
    public void Disable_SetsIsEnabledFalse()
    {
        ServiceRegistry.Register(MakeService("svc-a", enabled: true));
        ServiceRegistry.Disable("svc-a");
        Assert.False(ServiceRegistry.GetAll().First().IsEnabled);
    }

    [Fact]
    public void Enable_SetsIsEnabledTrue()
    {
        ServiceRegistry.Register(MakeService("svc-a", enabled: false));
        ServiceRegistry.Enable("svc-a");
        Assert.True(ServiceRegistry.GetAll().First().IsEnabled);
    }

    [Fact]
    public void Disable_UnknownId_DoesNotThrow()
    {
        ServiceRegistry.Register(MakeService("svc-a"));
        var ex = Record.Exception(() => ServiceRegistry.Disable("does-not-exist"));
        Assert.Null(ex);
    }

    // ── GetAvailableFor ──────────────────────────────────────────────────────

    [Fact]
    public void GetAvailableFor_ExcludesDisabledServices()
    {
        ServiceRegistry.Register(MakeService("on",  enabled: true));
        ServiceRegistry.Register(MakeService("off", enabled: false));
        var result = ServiceRegistry.GetAvailableFor(Role.User);
        Assert.Single(result);
        Assert.Equal("on", result[0].Id);
    }

    [Fact]
    public void GetAvailableFor_UserRole_CannotAccessAdminService()
    {
        ServiceRegistry.Register(MakeService("admin-only", enabled: true, minRole: Role.Admin));
        var result = ServiceRegistry.GetAvailableFor(Role.User);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAvailableFor_AdminRole_CanAccessAdminAndUserServices()
    {
        ServiceRegistry.Register(MakeService("for-users",  enabled: true, minRole: Role.User));
        ServiceRegistry.Register(MakeService("for-admins", enabled: true, minRole: Role.Admin));
        var result = ServiceRegistry.GetAvailableFor(Role.Admin);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetAvailableFor_RootRole_CanAccessAllServices()
    {
        ServiceRegistry.Register(MakeService("for-users",  enabled: true, minRole: Role.User));
        ServiceRegistry.Register(MakeService("for-admins", enabled: true, minRole: Role.Admin));
        ServiceRegistry.Register(MakeService("for-root",   enabled: true, minRole: Role.Root));
        var result = ServiceRegistry.GetAvailableFor(Role.Root);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetAvailableFor_UserRole_CanAccessUserService()
    {
        ServiceRegistry.Register(MakeService("for-users", enabled: true, minRole: Role.User));
        var result = ServiceRegistry.GetAvailableFor(Role.User);
        Assert.Single(result);
    }
}
