import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class LeaveTypeService {
  baseUrl: string = 'https://team2webapi.azurewebsites.net/api/leavetypes'

  constructor(private http: HttpClient) {

  }

  GetFromTimeRange<LeaveTypeDTO>(from: Date, to: Date): Observable<LeaveTypeDTO[]> {

    console.log(from)
    console.log(to)

    // Javascript dates skapades av djävulen.

    let fromYear = from.toLocaleString("default", { year: "numeric" });
    let fromMonth = from.toLocaleString("default", { month: "2-digit" });
    let fromDay = from.toLocaleString("default", { day: "2-digit" });

    let fromString = fromYear + "-" + fromMonth + "-" + fromDay

    let toYear = to.toLocaleString("default", { year: "numeric" });
    let toMonth = to.toLocaleString("default", { month: "2-digit" });
    let toDay = to.toLocaleString("default", { day: "2-digit" });

    let toString = toYear + "-" + toMonth + "-" + toDay

    return this.http.get<LeaveTypeDTO[]>(this.baseUrl + "/daysused/timerange" + "?from=" + fromString + "&to=" + toString)
  }

}
