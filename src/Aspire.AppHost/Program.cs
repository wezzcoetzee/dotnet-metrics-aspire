var builder = DistributedApplication.CreateBuilder(args);

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithVolumeMount("../grafana/config", "/etc/grafana")
    .WithVolumeMount("../grafana/dashboards", "/var/lib/grafana/dashboards")
    .WithEndpoint(containerPort: 3000, hostPort: 3000, name: "grafana-http", scheme: "http");

builder.AddContainer("prometheus", "prom/prometheus")
    .WithVolumeMount("../prometheus", "/etc/prometheus")
    .WithEndpoint(9090, hostPort: 9090);

var api = builder.AddProject<Projects.Weather_Api>("weatherapi")
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"));

builder.Build().Run();