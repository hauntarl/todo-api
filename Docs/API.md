# Todo API Specifications

This is a proposal for a **Todo** API that will allow the user to manage tasks on a to-do list. 
They will have to ability to create new tasks as well as update or delete existing ones.

## Endpoints

### Fetch All Tasks Endpoint

`GET /tasks`

**Parameters:**

- name: `?completed=bool`
- type: **bool**
- description: filters tasks by completed status

* name: `?sort_by=dueDate` *(Optional)*
* type: **string**
* description: sorts by due date (ascending)

- name: `?sort_by=-dueDate` *(Optional)*
- type: **string**
- description: sorts by due date (descending)

* name: `?sort_by=createdDate` *(Optional)*
* type: **string**
* description: sorts by created date (ascending)

- name: `?sort_by=-createdDate` *(Optional)*
- type: **string**
- description: sorts by created date (descending)

Response Code: `200` OK

Response Body:
```json
[
    {
        "id": GUID,
        "taskDescription": "**string**",
        "createdDate": "**string**",
        "dueDate": "**string**",
        "completed": bool
    },
    {
        "id": GUID,
        "taskDescription": "**string**",
        "createdDate": "**string**",
        "dueDate": "**string**",
        "completed": bool
    },
    {
        "id": GUID,
        "taskDescription": "**string**",
        "createdDate": "**string**",
        "dueDate": "**string**",
        "completed": bool
    },
    ...
]
```

Failure States: `4XX` Client Error

```json
{
    message: "**string**"
}
```

### Fetch Task Endpoint

`GET /tasks/{id}`

Response Code: `200` OK

Response Body:
```json

{
    "id": GUID,
    "taskDescription": "**string**",
    "createdDate": "**string**",
    "dueDate": "**string**",
    "completed": bool
}
```

Failure States: `4XX` Client Error
```json
{
    message: "**string**"
}
```

### Create Task Endpoint

`POST /tasks`

Request Body:
```json
{
    "taskDescription": "**string**",
    "dueDate": "**string**",
    "completed": bool
}
```

Response Code: `201` CREATED

Response Body:
```json
{
    "id": GUID,
    "taskDescription": "**string**",
    "createdDate": "**string**",
    "dueDate": "**string**",
    "completed": bool
}
```

Failure States: `4XX` Client Error
```json
{
    message: "**string**"
}
```

### Update Task Endpoint

`PUT /tasks/{id}`

Request Body:
```json
{
    "id": GUID,                      //immutable
    "taskDescription": "**string**",
    "createdDate": "**string**",         //immutable
    "dueDate": "**string**",
    "completed": bool
}
```

Response Code: `200` OK

Response Body:
```json
{
    "id": GUID,
    "taskDescription": "**string**",
    "createdDate": "**string**",
    "dueDate": "**string**",
    "completed": bool
}
```

Failure States: `4XX` Client Error
```json
{
    message: "**string**"
}
```

### Delete Task Endpoint

`DELETE /tasks/{id}`

Repsonse Code: `200` OK

Response Body:

Failure States: `4XX` Client Error
```json
{
    message: "**string**"
}
```
