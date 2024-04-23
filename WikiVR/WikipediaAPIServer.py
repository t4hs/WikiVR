import socket
import wikipedia

def get_wikipedia_content(topic):
    try:
        # Get the Wikipedia page
        page = wikipedia.page(topic)

        # Return the content
        return f"===Title===\n{page.title}\n\n===Content===\n{page.content}\n"

    except wikipedia.exceptions.PageError:
        return f"Page not found for '{topic}'"
    except wikipedia.exceptions.DisambiguationError as e:
        return f"Ambiguous term: '{topic}'. Suggestions: {e.options}"

def get_wikipedia_images(topic):
    try:
        # Get the Wikipedia page
        page = wikipedia.page(topic)

        # Return the images
        images = "\n".join(page.images)
        return f"===Images===\n{images}"

    except wikipedia.exceptions.PageError:
        return f"Page not found for '{topic}'"
    except wikipedia.exceptions.DisambiguationError as e:
        return f"Ambiguous term: '{topic}'. Suggestions: {e.options}"

def get_wikipedia_links(topic):
    try:
        # Get the Wikipedia page
        page = wikipedia.page(topic)

        # Return the links
        links = "\n".join(page.links)
        return f"===Links===\n{links}"

    except wikipedia.exceptions.PageError:
        return f"Page not found for '{topic}'"
    except wikipedia.exceptions.DisambiguationError as e:
        return f"Ambiguous term: '{topic}'. Suggestions: {e.options}"

host = '127.0.0.1'  # localhost
port = 5555

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((host, port))
server_socket.listen(1)

print(f"Listening on {host}:{port}")

while True:
    client_socket, client_address = server_socket.accept()
    print(f"Connection from {client_address}")

    # Receive the topic from the Unity client
    topic_to_send = client_socket.recv(1024).decode('utf-8')
    print(f"Received topic: {topic_to_send}")

    # Send Wikipedia title to the Unity client
    title_message = get_wikipedia_content(topic_to_send).split("===Title===\n")[1]
    client_socket.sendall(title_message.encode('utf-8'))

    # Send Wikipedia content to the Unity client
    content_message = get_wikipedia_content(topic_to_send)
    client_socket.sendall(content_message.encode('utf-8'))

    # Send Wikipedia images to the Unity client
    images_message = get_wikipedia_images(topic_to_send)
    client_socket.sendall(images_message.encode('utf-8'))

    # Send Wikipedia links to the Unity client
    links_message = get_wikipedia_links(topic_to_send)
    client_socket.sendall(links_message.encode('utf-8'))

    # Send the end message
    end_message = "===END==="
    client_socket.sendall(end_message.encode('utf-8'))

    client_socket.close()

server_socket.close()
