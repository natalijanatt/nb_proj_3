# WorkspaceMonitor

Aplikacija za praćenje sistemskih resursa u realnom vremenu. Skuplja podatke o CPU, RAM-u, bateriji, diskovima i mreži svake sekunde, šalje ih u InfluxDB, a prikaz je omogućen kroz Grafana dashboard.

## Pokretanje

**Potrebno:** Docker, .NET 8 SDK

1. Kopirati `.env.example` u `.env` i popuniti token:
   ```
   INFLUX_TOKEN=<tvoj_token>
   ```


2. Pokrenuti infrastrukturu (InfluxDB + Grafana):
   ```bash
   cd docker
   docker compose up -d
   ```

3. Pokrenuti aplikaciju:
   ```bash
   dotnet run
   ```

Grafana je dostupna na `http://localhost:3000`, InfluxDB UI na `http://localhost:8080`.

## Pregled u grafani

Ulogovati se sa kredencijalima:
```
usernam: admin
password: admin12345
```

U sekciji _Dashboards_ nalazi se **_Workspace Monitor_**, ili direktno: [Workspace Monitor Dashboard](http://localhost:3000/d/dfe0tda9jwvswf/workspace-monitor?var-cpu_core_num=$__all)

Tu se nalaze svi grafikoni za prikaz podataka o sistemu u realnom vremenu.

## Struktura

```
Services/
  BackgroundWorker/   # petlja koja svakih 1s poziva sve servise
  Processing/         # po jedan servis za svaki tip metrike (CPU, RAM, baterija, disk, mreža)
  HwStatsProvider/    # apstrakcija oko Hardware.Info biblioteke
  Influx/             # klijent za pisanje u InfluxDB
Dtos/                 # modeli podataka
docker/               # docker-compose + Grafana konfiguracija i dashboardovi
```
