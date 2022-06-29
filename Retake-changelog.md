# 1. Updated class diagram
 - Added relational mapping to classes in class diagram.
 - Added technical design description and explanation of relationships

# 2. Updated README.md
  - Added prerequisites for docker installation
  - Added step description to provide more context

# 3. Improved error handling
  - Add ErrorHandler: global error handler that assures any error will be catched before returning the reponse
  - Refactored Controllers: controllers do not contain a try catch block anymore
  - Refactored Services: the error handling is delegated to the services
  - Removed usage of base "Exception" class, replaced erros with either PopUpNowException for bad input and NotFoundException.

# 4. Updated Word document
  - Cleanup and restructured document
    
# 5. Improved and added comments