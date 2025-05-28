from flask import Flask, request, jsonify
import socket
import subprocess
import time

servers_ready = []


app = Flask(__name__)


@app.route('/')
def hello():
    return 'Hello from Flask!'


@app.route('/unity/start', methods=['GET'])
def server_start():
    # Optional: read query parameters from Unity
    player_name = request.args.get('name', 'Player')

    # You can perform any logic here
    print(f"Received request from Unity client: {player_name}")

    port = get_available_port()
    print(f"Random available port: {port}")

    run_unity_headless(r"F:\Git\NetworkingTests\ServerBuild\VRTemplate.exe", ["ServerInstance", f'port:{port}'])

    while True:
        if port in servers_ready:
            print("Port found in server ready")
            break
        else:
            print(f"Port {port} not in server ready")
            time.sleep(0.25)

    # Return a JSON response
    return jsonify({
        'address': 'localhost',
        'port': port
    })


@app.route('/unity/servers', methods=['GET'])
def get_servers():
    print(f"Received request for servers available")
    json = jsonify([{'address': 'localhost', 'port': x} for x in servers_ready])
    print(json)
    return json


@app.route('/unity/ready', methods=['GET'])
def server_ready():
    # Optional: read query parameters from Unity
    server_port = request.args.get('port', '8080')

    print(f"Received server ready from Unity server: {server_port}")

    servers_ready.append(int(server_port))

    return "ACK"


def get_available_port():
    s = socket.socket()
    s.bind(('', 0))
    port = s.getsockname()[1]
    s.close()
    return port


def run_unity_headless(executable_path, arguments=None):
    """
    Run a Unity executable in headless mode with optional arguments.

    Args:
        executable_path (str): Full path to the Unity executable.
        arguments (list or None): List of additional command-line args to pass.
    """
    if arguments is None:
        arguments = []

    # Unity command line args for headless mode:
    # -batchmode: run Unity in batch (no graphics) mode
    # -nographics: disables graphics device
    # -quit: quit after execution (optional depending on your build)

    cmd = [
              executable_path,
              '-logFile', 'unity_headless.log',  # optional: log output to a file
              # '-quit'  # Uncomment if you want Unity to exit automatically after running
          ] + arguments

    print(f"Running Unity headless with command: {' '.join(cmd)}")

    # Run process, redirect output to console (or change to subprocess.PIPE)
    process = subprocess.Popen(cmd)
    return process  # You can later call process.wait() or process.terminate()


if __name__ == '__main__':
    # Run on localhost:5000
    app.run(host='0.0.0.0', port=8080, debug=True)