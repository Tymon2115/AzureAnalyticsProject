import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { StatisticsService } from '../../services/statistics.service';
import { Subject, takeUntil } from 'rxjs';
import { StatisticEntry } from '../../models/statistics-entry.model';
import { format } from 'date-fns';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgxChartsModule],
  providers: [StatisticsService],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit, OnDestroy {
  public statisticsData: StatisticEntry[] = [];
  private unsubscribe$ = new Subject<void>();
  public loaded = false;

  chartData: any[] = [];
  colorScheme = {
    domain: ['#5AA454', '#FF5252'],
  };

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit(): void {
    this.statisticsService
      .getStatistics()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: (data) => {
          if (data && data.length > 0) {
            this.loadChartData(data);
          }
        },
        error: (err) => {
          console.error('Error fetching statistics:', err);
        },
      });
  }

  private loadChartData(statistics: StatisticEntry[]): void {
    const bugs = {
      name: 'Bugs',
      series: statistics.map((entry) => ({
        name: entry.date.toString(),
        value: entry.bugCount,
      })),
    };

    const features = {
      name: 'Features',
      series: statistics.map((entry) => ({
        name: entry.date.toString(),
        value: entry.featureCount,
      })),
    };
    this.chartData = [bugs, features];
    this.loaded = true;
  }

  public formatXAxisTick(value: string | Date): string {
    const date = new Date(value);
    return format(date, 'HH:mm');
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
