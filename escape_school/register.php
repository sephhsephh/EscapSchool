<?php
require 'config.php';

$data = json_decode(file_get_contents("php://input"));

$email = $data->email;
$password = password_hash($data->password, PASSWORD_DEFAULT);
$firstName = isset($data->firstName) ? $data->firstName : null;
$lastName = isset($data->lastName) ? $data->lastName : null;
$role = isset($data->role) ? $data->role : 'user';
$verified = isset($data->verified) ? (int)$data->verified : 0;

// Check if email already exists
$check = $conn->prepare("SELECT id FROM users WHERE email = ?");
$check->bind_param("s", $email);
$check->execute();
$check->store_result();

if ($check->num_rows > 0) {
    echo json_encode(["success" => false, "message" => "Email already registered"]);
    $check->close();
    $conn->close();
    exit();
}
$check->close();

// Insert new user
$stmt = $conn->prepare("INSERT INTO users (email, password, first_name, last_name, role, verified) VALUES (?, ?, ?, ?, ?, ?)");
$stmt->bind_param("sssssi", $email, $password, $firstName, $lastName, $role, $verified);

if ($stmt->execute()) {
    echo json_encode(["success" => true, "message" => "User registered successfully"]);
} else {
    echo json_encode(["success" => false, "message" => "Registration failed: " . $conn->error]);
}

$stmt->close();
$conn->close();
?>