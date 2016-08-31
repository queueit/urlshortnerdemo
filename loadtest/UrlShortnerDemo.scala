
import scala.concurrent.duration._

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import java.util.concurrent.ThreadLocalRandom

class UrlShortnerDemo extends Simulation {

  val longUrl = "http://docs.aws.amazon.com/codedeploy/latest/userguide/elastic-load-balancing-integ.html"

  val feeder = Iterator.continually(Map(
    "longUrl" -> longUrl))

	val httpProtocol = http
		.baseURL("http://url.realvaluetalks.com")
    .disableFollowRedirect

	val scn = scenario("UrlShortnerDemo")
      .feed(feeder)
      .exec(flushCookieJar)
      .exitBlockOnFail(
        exec(http("Home Page")
    	  .get("/")
          .check(status.is(200)))
        .exec(http("Shorten Url")
          .post("/")
          .headers(Map("Content-Type" -> "application/x-www-form-urlencoded"))
          .body(StringBody("Url=${longUrl}"))
          .check(status.in(200))
          .check(regex("""href=".*/in/(.{8})"""").find.optional.saveAs("urlId")))
      )
      .repeat(100) {
        pause(6 seconds)
        .exec(http("Lookup")
          .get("/in/${urlId}")
          .headers(Map("User-Agent" -> "Load test agent"))
          .check(status.is(302)))
      }

	setUp(scn.inject(rampUsers(2000) over (600 seconds))).protocols(httpProtocol)
}
