# Todo API Specifications

This is a proposal for a **Todo** API that will allow the user to manage tasks on a to-do list. 
They will have to ability to create new tasks as well as update or delete existing ones.

## Endpoints

### Fetch All Tasks Endpoint

```js
GET /tasks
```

#### Parameters

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

#### Success

```js
200 OK
```
```json
[
    {
        "id": "800513d7-1c11-416e-8287-8480cb41accd",
        "taskDescription": "Grocery Shopping",
        "createdDate": "2024-04-21T02:32:39.014829Z",
        "dueDate": "2024-04-30T11:00:00Z",
        "completed": false
    },
    {
        "id": "0095da82-5a1f-4715-ad0f-e05347c58c41",
        "taskDescription": "Clean House",
        "createdDate": "2024-04-21T02:33:11.346353Z",
        "dueDate": "2024-05-10T15:00:00Z",
        "completed": false
    },
    {
        "id": "f328e10e-141f-4f0d-8e24-767cb20b5baf",
        "taskDescription": "Blog Post",
        "createdDate": "2024-04-21T02:33:54.172113Z",
        "dueDate": "2024-05-20T15:00:00Z",
        "completed": false
    }
]
```

#### Failure

```
<empty>
```

### Fetch Task Endpoint

```js
GET /tasks/{id}
```

#### Success

```js
200 OK
```
```json
{
    "id": "f328e10e-141f-4f0d-8e24-767cb20b5baf",
    "taskDescription": "Blog Post",
    "createdDate": "2024-04-21T02:33:54.172113Z",
    "dueDate": "2024-05-20T15:00:00Z",
    "completed": false
}
```

#### Failure

```js
404 Not Found
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
    "title": "Task not found",
    "status": 404,
    "traceId": "00-67084c94108517c7b13dd97ebb1c171f-ecfe7623d119970b-00"
}
```

### Create Task Endpoint

```js
POST /tasks
```
```json
{
    "taskDescription": "Grocery Shopping",
    "dueDate": "2024-04-30T11:00:00.000000Z",
    "completed": false
}
```

#### Success

```js
201 Created

Location: {{host}}/tasks/e429ba35-5cd0-4fe9-856c-50a582ad79a2
```
```json
{
    "id": "e429ba35-5cd0-4fe9-856c-50a582ad79a2",
    "taskDescription": "Grocery Shopping",
    "createdDate": "2024-04-21T02:44:10.641677Z",
    "dueDate": "2024-04-30T11:00:00Z",
    "completed": false
}
```

#### Failure

```js
400 Bad Request
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "Task description must be atleast 3 characters long",
    "status": 400,
    "traceId": "00-b11ab92e6ea9715b4cef612a9bf721c5-0df83cb0b13f678a-00"
}
```

### Update Task Endpoint

```js
PUT /tasks/{id}
```
```json
{
    "id": "8260dbd0-1ce8-4c45-b8af-aeb09221945b",
    "taskDescription": "Blog Post",
    "createdDate": "2024-04-21T02:47:37.223515Z",
    "dueDate": "2024-05-20T00:00:00Z",
    "completed": true
}
```

#### Success

```js
204 No Content
```
```
<empty>
```

```js
201 Created

Location: {{host}}/tasks/e429ba35-5cd0-4fe9-856c-50a582ad79a2
```
```json
{
    "id": "e429ba35-5cd0-4fe9-856c-50a582ad79a2",
    "taskDescription": "Grocery Shopping",
    "createdDate": "2024-04-21T02:44:10.641677Z",
    "dueDate": "2024-04-30T11:00:00Z",
    "completed": false
}
```

#### Failure

```js
400 Bad Request
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "Task description must be atleast 3 characters long",
    "status": 400,
    "traceId": "00-ac98218f107e9587615a4641bfb284c5-2ff0865aea658198-00"
}
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "Task id must be the same.",
    "status": 400,
    "traceId": "00-176b26ddfa2700a7b14b7da4a742d0a9-c1f7dd7fc14cb973-00"
}
```

### Delete Task Endpoint

```js
DELETE /tasks/{id}
```

#### Success

```js
204 No Content
```
```
<empty>
```

## Interval Server Error

```js
500 Internal server error
```
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
    "title": "An error occurred while processing your request.",
    "status": 500,
    "traceId": "00-b637cabd3f4c910ebfa87b2df4e4d425-79196a3af1692da1-00"
}
```
