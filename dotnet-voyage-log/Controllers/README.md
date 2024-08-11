# Controllers
This folder contains app controllers. 

 - ImageController: Admin-authorized Controller provides REST API interface for image operations. Voyage objects contain image IDs so they can be retrieved from this controller.
    * GET /Image: Retrieves all images from S3
    * GET /Image/{key}: Retrieves single image by key from S3
    * PUT /Image: Insert one or more images as a list to S3. Expects Image model from form.
    * DELETE /Image/{key}: Deletes one or more images as a list from S3
 - IndexController: Non-authorized Controller. Server up message for health checks
    * GET: Retrieves information string, that indicates server is running
 - LocationController: Non-authorized Controller provides REST API interface for countries and regions.
    * GET /Location/Countries: Retrieves all countries from database.
    * GET /Location/Regions: Retrieves all regions from database.
    * GET /Location/Countries/{name}: Retrieves a single country by name from database.
    * GET /Location/Regions/{name}: Retrieves a single region by name from database.
 - SignupController: Non-authorized Controller provides login interface
    * POST /Signup: Creates a normal user without admin rights to database. Expect SignupUser model from body.
 - UserController: Admin-authorized Controller provides REST API interface for user management.
    * GET /User: Retrieves all users from database.
    * GET /User/{id}: Retrieves single user by id from database.
    * POST /User: Insert new user with admin rights to database. Excepts SignupUser model from body.
    * PUT /User/{id}: Updates user by id to database. Expects User model from body and also id from url parameter
    * DELETE /User/{id}: Deletes single user by id from database.
 - VoyageController: Admin/User-authorized Controller provides REST API interface for voyage-log objects.
    * GET /Voyage: Retrieves all Voyage-logs from database.
    * POST /Voyage: Insert new voyage-log to database. Expects Voyage model from body.
    * PUT /Voyage/{id}: Update voyage-log by id to database. Expects Voyage model from body and also id from url parameter
    * DELETE /Voyage/{id}: Removes single voyage-log by id from database.
