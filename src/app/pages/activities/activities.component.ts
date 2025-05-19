import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivityService, ActivityLog } from '../../services/activity.service';

@Component({
  selector: 'app-activities',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.css']
})
export class ActivitiesComponent implements OnInit {
  activities: ActivityLog[] = [];

  constructor(private activityService: ActivityService) {}

  ngOnInit(): void {
    this.activityService.getActivities().subscribe({
      next: data => this.activities = data,
      error: err => console.error('Aktiviteler alınamadı:', err)
    });
  }
}
